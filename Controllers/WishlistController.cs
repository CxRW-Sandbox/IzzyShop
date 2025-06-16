using Microsoft.AspNetCore.Mvc;
using IzzyShop.Models;
using IzzyShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public WishlistController(ApplicationDbContext context) { _context = context; }

        // Vulnerable: SQL Injection
        [HttpGet("find-by-description")]
        public IActionResult FindByDescription(string desc)
        {
            var sql = $"SELECT * FROM Wishlists WHERE Description = '{desc}'";
            var wishlists = _context.Set<Wishlist>().FromSqlRaw(sql).ToList();
            return Ok(wishlists);
        }

        // Vulnerable: Stored XSS in notes
        [HttpPost("add-note/{wishlistId}")]
        public IActionResult AddNote(int wishlistId, [FromBody] string note)
        {
            var wishlist = _context.Set<Wishlist>().Find(wishlistId);
            if (wishlist == null) return NotFound();
            wishlist.Notes = note;
            _context.SaveChanges();
            return Ok(wishlist);
        }

        // Vulnerable: XXE in metadata
        [HttpPost("parse-metadata/{wishlistId}")]
        public IActionResult ParseMetadata(int wishlistId)
        {
            var wishlist = _context.Set<Wishlist>().Find(wishlistId);
            if (wishlist == null) return NotFound();
            var xml = wishlist.Metadata;
            var doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver();
            doc.LoadXml(xml);
            return Ok(doc.InnerXml);
        }

        // Vulnerable: IDOR - get wishlist by id
        [HttpGet("{id}")]
        public IActionResult GetWishlist(int id)
        {
            var wishlist = _context.Set<Wishlist>().Find(id);
            if (wishlist == null) return NotFound();
            return Ok(wishlist);
        }

        // Add more endpoints for bloat
        [HttpPost("add-item")] public IActionResult AddItem([FromBody] WishlistItem item) { return Ok(item); }
        [HttpPost("remove-item")] public IActionResult RemoveItem([FromBody] WishlistItem item) { return Ok(item); }
        [HttpPost("share")] public IActionResult Share([FromBody] int wishlistId) { return Ok(wishlistId); }
        [HttpGet("all")] public IActionResult All() { return Ok(_context.Set<Wishlist>().ToList()); }
        [HttpPost("clear")] public IActionResult Clear([FromBody] int wishlistId) { return Ok(wishlistId); }
    }
} 