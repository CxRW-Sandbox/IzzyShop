using Microsoft.AspNetCore.Mvc;
using IzzyShop.Models;
using IzzyShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ShippingController(ApplicationDbContext context) { _context = context; }

        // Vulnerable: SQL Injection
        [HttpGet("find-by-status")]
        public IActionResult FindByStatus(string status)
        {
            var sql = $"SELECT * FROM Shippings WHERE Status = '{status}'";
            var shippings = _context.Set<Shipping>().FromSqlRaw(sql).ToList();
            return Ok(shippings);
        }

        // Vulnerable: Stored XSS in notes
        [HttpPost("add-note/{shippingId}")]
        public IActionResult AddNote(int shippingId, [FromBody] string note)
        {
            var shipping = _context.Set<Shipping>().Find(shippingId);
            if (shipping == null) return NotFound();
            shipping.Notes = note;
            _context.SaveChanges();
            return Ok(shipping);
        }

        // Vulnerable: XXE in metadata
        [HttpPost("parse-metadata/{shippingId}")]
        public IActionResult ParseMetadata(int shippingId)
        {
            var shipping = _context.Set<Shipping>().Find(shippingId);
            if (shipping == null) return NotFound();
            var xml = shipping.Metadata;
            var doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver();
            doc.LoadXml(xml);
            return Ok(doc.InnerXml);
        }

        // Vulnerable: IDOR - get shipping by id
        [HttpGet("{id}")]
        public IActionResult GetShipping(int id)
        {
            var shipping = _context.Set<Shipping>().Find(id);
            if (shipping == null) return NotFound();
            return Ok(shipping);
        }

        // Add more endpoints for bloat
        [HttpPost("add-event")] public IActionResult AddEvent([FromBody] ShippingEvent ev) { return Ok(ev); }
        [HttpPost("remove-event")] public IActionResult RemoveEvent([FromBody] ShippingEvent ev) { return Ok(ev); }
        [HttpPost("update-status")] public IActionResult UpdateStatus([FromBody] int shippingId) { return Ok(shippingId); }
        [HttpGet("all")] public IActionResult All() { return Ok(_context.Set<Shipping>().ToList()); }
        [HttpPost("clear")] public IActionResult Clear([FromBody] int shippingId) { return Ok(shippingId); }
    }
} 