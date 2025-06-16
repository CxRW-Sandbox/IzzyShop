using Microsoft.AspNetCore.Mvc;
using IzzyShop.Models;
using IzzyShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PaymentController(ApplicationDbContext context) { _context = context; }

        // Vulnerable: SQL Injection
        [HttpGet("find-by-status")]
        public IActionResult FindByStatus(string status)
        {
            var sql = $"SELECT * FROM Payments WHERE Status = '{status}'";
            var payments = _context.Set<Payment>().FromSqlRaw(sql).ToList();
            return Ok(payments);
        }

        // Vulnerable: Stored XSS in notes
        [HttpPost("add-note/{paymentId}")]
        public IActionResult AddNote(int paymentId, [FromBody] string note)
        {
            var payment = _context.Set<Payment>().Find(paymentId);
            if (payment == null) return NotFound();
            payment.Notes = note;
            _context.SaveChanges();
            return Ok(payment);
        }

        // Vulnerable: XXE in metadata
        [HttpPost("parse-metadata/{paymentId}")]
        public IActionResult ParseMetadata(int paymentId)
        {
            var payment = _context.Set<Payment>().Find(paymentId);
            if (payment == null) return NotFound();
            var xml = payment.Metadata;
            var doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver();
            doc.LoadXml(xml);
            return Ok(doc.InnerXml);
        }

        // Vulnerable: IDOR - get payment by id
        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            var payment = _context.Set<Payment>().Find(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        // Vulnerable: Sensitive data exposure
        [HttpGet("card/{id}")]
        public IActionResult GetCard(int id)
        {
            var payment = _context.Set<Payment>().Find(id);
            if (payment == null) return NotFound();
            return Ok(new { payment.CardNumber, payment.CardExpiry, payment.CardCvv });
        }

        // Add more endpoints for bloat
        [HttpPost("pay")] public IActionResult Pay([FromBody] Payment payment) { return Ok(payment); }
        [HttpPost("refund")] public IActionResult Refund([FromBody] int paymentId) { return Ok(paymentId); }
        [HttpGet("all")] public IActionResult All() { return Ok(_context.Set<Payment>().ToList()); }
        [HttpPost("clear")] public IActionResult Clear([FromBody] int paymentId) { return Ok(paymentId); }
    }
} 