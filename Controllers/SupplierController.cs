using Microsoft.AspNetCore.Mvc;
using IzzyShop.Models;
using IzzyShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SupplierController(ApplicationDbContext context) { _context = context; }

        // Vulnerable: SQL Injection
        [HttpGet("find-by-name")]
        public IActionResult FindByName(string name)
        {
            var sql = $"SELECT * FROM Suppliers WHERE Name = '{name}'";
            var suppliers = _context.Set<Supplier>().FromSqlRaw(sql).ToList();
            return Ok(suppliers);
        }

        // Vulnerable: Stored XSS in notes
        [HttpPost("add-note/{supplierId}")]
        public IActionResult AddNote(int supplierId, [FromBody] string note)
        {
            var supplier = _context.Set<Supplier>().Find(supplierId);
            if (supplier == null) return NotFound();
            supplier.Notes = note;
            _context.SaveChanges();
            return Ok(supplier);
        }

        // Vulnerable: XXE in metadata
        [HttpPost("parse-metadata/{supplierId}")]
        public IActionResult ParseMetadata(int supplierId)
        {
            var supplier = _context.Set<Supplier>().Find(supplierId);
            if (supplier == null) return NotFound();
            var xml = supplier.Metadata;
            var doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver();
            doc.LoadXml(xml);
            return Ok(doc.InnerXml);
        }

        // Vulnerable: IDOR - get supplier by id
        [HttpGet("{id}")]
        public IActionResult GetSupplier(int id)
        {
            var supplier = _context.Set<Supplier>().Find(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        // Vulnerable: Sensitive data exposure
        [HttpGet("bank/{id}")]
        public IActionResult GetBankAccount(int id)
        {
            var supplier = _context.Set<Supplier>().Find(id);
            if (supplier == null) return NotFound();
            return Ok(new { supplier.BankAccount });
        }

        // Add more endpoints for bloat
        [HttpPost("activate")] public IActionResult Activate([FromBody] int supplierId) { return Ok(supplierId); }
        [HttpPost("deactivate")] public IActionResult Deactivate([FromBody] int supplierId) { return Ok(supplierId); }
        [HttpGet("all")] public IActionResult All() { return Ok(_context.Set<Supplier>().ToList()); }
        [HttpPost("clear")] public IActionResult Clear([FromBody] int supplierId) { return Ok(supplierId); }
    }
} 