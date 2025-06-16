using Microsoft.AspNetCore.Mvc;
using IzzyShop.Models;
using IzzyShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context) { _context = context; }

        // Vulnerable: SQL Injection
        [HttpGet("find-by-note")]
        public IActionResult FindByNote(string note)
        {
            var sql = $"SELECT * FROM Carts WHERE Notes = '{note}'";
            var carts = _context.Set<Cart>().FromSqlRaw(sql).ToList();
            return Ok(carts);
        }

        // Vulnerable: Stored XSS in notes
        [HttpPost("add-note/{cartId}")]
        public IActionResult AddNote(int cartId, [FromBody] string note)
        {
            var cart = _context.Set<Cart>().Find(cartId);
            if (cart == null) return NotFound();
            cart.Notes = note;
            _context.SaveChanges();
            return Ok(cart);
        }

        // Vulnerable: XXE in metadata
        [HttpPost("parse-metadata/{cartId}")]
        public IActionResult ParseMetadata(int cartId)
        {
            var cart = _context.Set<Cart>().Find(cartId);
            if (cart == null) return NotFound();
            var xml = cart.Metadata;
            var doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver();
            doc.LoadXml(xml);
            return Ok(doc.InnerXml);
        }

        // Vulnerable: IDOR - get cart by id
        [HttpGet("{id}")]
        public IActionResult GetCart(int id)
        {
            var cart = _context.Set<Cart>().Find(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        // Add more endpoints for bloat
        [HttpPost("add-item")] public IActionResult AddItem([FromBody] CartItem item) { return Ok(item); }
        [HttpPost("remove-item")] public IActionResult RemoveItem([FromBody] CartItem item) { return Ok(item); }
        [HttpPost("checkout")] public IActionResult Checkout([FromBody] int cartId) { return Ok(cartId); }
        [HttpGet("all")] public IActionResult All() { return Ok(_context.Set<Cart>().ToList()); }
        [HttpPost("clear")] public IActionResult Clear([FromBody] int cartId) { return Ok(cartId); }
    }
} 