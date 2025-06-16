using Microsoft.AspNetCore.Mvc;
using IzzyShop.Models;
using IzzyShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public InventoryController(ApplicationDbContext context) { _context = context; }

        // Vulnerable: SQL Injection
        [HttpGet("find-by-location")]
        public IActionResult FindByLocation(string location)
        {
            var sql = $"SELECT * FROM Inventories WHERE Location = '{location}'";
            var inventories = _context.Set<Inventory>().FromSqlRaw(sql).ToList();
            return Ok(inventories);
        }

        // Vulnerable: Stored XSS in notes
        [HttpPost("add-note/{inventoryId}")]
        public IActionResult AddNote(int inventoryId, [FromBody] string note)
        {
            var inventory = _context.Set<Inventory>().Find(inventoryId);
            if (inventory == null) return NotFound();
            inventory.Notes = note;
            _context.SaveChanges();
            return Ok(inventory);
        }

        // Vulnerable: XXE in metadata
        [HttpPost("parse-metadata/{inventoryId}")]
        public IActionResult ParseMetadata(int inventoryId)
        {
            var inventory = _context.Set<Inventory>().Find(inventoryId);
            if (inventory == null) return NotFound();
            var xml = inventory.Metadata;
            var doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver();
            doc.LoadXml(xml);
            return Ok(doc.InnerXml);
        }

        // Vulnerable: IDOR - get inventory by id
        [HttpGet("{id}")]
        public IActionResult GetInventory(int id)
        {
            var inventory = _context.Set<Inventory>().Find(id);
            if (inventory == null) return NotFound();
            return Ok(inventory);
        }

        // Add more endpoints for bloat
        [HttpPost("update-quantity")] public IActionResult UpdateQuantity([FromBody] int inventoryId) { return Ok(inventoryId); }
        [HttpPost("move")] public IActionResult Move([FromBody] int inventoryId) { return Ok(inventoryId); }
        [HttpGet("all")] public IActionResult All() { return Ok(_context.Set<Inventory>().ToList()); }
        [HttpPost("clear")] public IActionResult Clear([FromBody] int inventoryId) { return Ok(inventoryId); }
    }
} 