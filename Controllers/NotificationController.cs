using Microsoft.AspNetCore.Mvc;
using IzzyShop.Models;
using IzzyShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public NotificationController(ApplicationDbContext context) { _context = context; }

        // Vulnerable: SQL Injection
        [HttpGet("find-by-type")]
        public IActionResult FindByType(string type)
        {
            var sql = $"SELECT * FROM Notifications WHERE Type = '{type}'";
            var notifications = _context.Set<Notification>().FromSqlRaw(sql).ToList();
            return Ok(notifications);
        }

        // Vulnerable: Stored XSS in message
        [HttpPost("add-message/{notificationId}")]
        public IActionResult AddMessage(int notificationId, [FromBody] string message)
        {
            var notification = _context.Set<Notification>().Find(notificationId);
            if (notification == null) return NotFound();
            notification.Message = message;
            _context.SaveChanges();
            return Ok(notification);
        }

        // Vulnerable: XXE in metadata
        [HttpPost("parse-metadata/{notificationId}")]
        public IActionResult ParseMetadata(int notificationId)
        {
            var notification = _context.Set<Notification>().Find(notificationId);
            if (notification == null) return NotFound();
            var xml = notification.Metadata;
            var doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver();
            doc.LoadXml(xml);
            return Ok(doc.InnerXml);
        }

        // Vulnerable: IDOR - get notification by id
        [HttpGet("{id}")]
        public IActionResult GetNotification(int id)
        {
            var notification = _context.Set<Notification>().Find(id);
            if (notification == null) return NotFound();
            return Ok(notification);
        }

        // Vulnerable: Open Redirect
        [HttpGet("redirect/{id}")]
        public IActionResult RedirectToLink(int id)
        {
            var notification = _context.Set<Notification>().Find(id);
            if (notification == null) return NotFound();
            return Redirect(notification.Link); // No validation
        }

        // Add more endpoints for bloat
        [HttpPost("mark-read")] public IActionResult MarkRead([FromBody] int notificationId) { return Ok(notificationId); }
        [HttpPost("delete")] public IActionResult Delete([FromBody] int notificationId) { return Ok(notificationId); }
        [HttpGet("all")] public IActionResult All() { return Ok(_context.Set<Notification>().ToList()); }
        [HttpPost("clear")] public IActionResult Clear([FromBody] int notificationId) { return Ok(notificationId); }
    }
} 