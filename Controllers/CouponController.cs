using Microsoft.AspNetCore.Mvc;
using IzzyShop.Models;
using IzzyShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CouponController(ApplicationDbContext context) { _context = context; }

        // Vulnerable: SQL Injection
        [HttpGet("find-by-code")]
        public IActionResult FindByCode(string code)
        {
            var sql = $"SELECT * FROM Coupons WHERE Code = '{code}'";
            var coupons = _context.Set<Coupon>().FromSqlRaw(sql).ToList();
            return Ok(coupons);
        }

        // Vulnerable: Stored XSS in description
        [HttpPost("add-description/{couponId}")]
        public IActionResult AddDescription(int couponId, [FromBody] string desc)
        {
            var coupon = _context.Set<Coupon>().Find(couponId);
            if (coupon == null) return NotFound();
            coupon.Description = desc;
            _context.SaveChanges();
            return Ok(coupon);
        }

        // Vulnerable: XXE in metadata
        [HttpPost("parse-metadata/{couponId}")]
        public IActionResult ParseMetadata(int couponId)
        {
            var coupon = _context.Set<Coupon>().Find(couponId);
            if (coupon == null) return NotFound();
            var xml = coupon.Metadata;
            var doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver();
            doc.LoadXml(xml);
            return Ok(doc.InnerXml);
        }

        // Vulnerable: IDOR - get coupon by id
        [HttpGet("{id}")]
        public IActionResult GetCoupon(int id)
        {
            var coupon = _context.Set<Coupon>().Find(id);
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }

        // Add more endpoints for bloat
        [HttpPost("activate")] public IActionResult Activate([FromBody] int couponId) { return Ok(couponId); }
        [HttpPost("deactivate")] public IActionResult Deactivate([FromBody] int couponId) { return Ok(couponId); }
        [HttpGet("all")] public IActionResult All() { return Ok(_context.Set<Coupon>().ToList()); }
        [HttpPost("clear")] public IActionResult Clear([FromBody] int couponId) { return Ok(couponId); }
    }
} 