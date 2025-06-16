using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IzzyShop.Data;
using IzzyShop.Models;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using System.Net.Http;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Vulnerable: SQL Injection
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            // Vulnerable: Direct string concatenation in SQL query
            var sql = $"SELECT * FROM Products WHERE Name LIKE '%{query}%' OR Description LIKE '%{query}%'";
            var products = await _context.Products.FromSqlRaw(sql).ToListAsync();
            return Ok(products);
        }

        // Vulnerable: XXE Injection
        [HttpPost("parse-metadata")]
        public IActionResult ParseMetadata([FromBody] string xmlContent)
        {
            // Vulnerable: No XXE protection
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            return Ok(xmlDoc.InnerXml);
        }

        // Vulnerable: Path Traversal
        [HttpGet("image")]
        public IActionResult GetImage(string path)
        {
            // Vulnerable: No path validation
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", path);
            if (System.IO.File.Exists(fullPath))
            {
                return PhysicalFile(fullPath, "image/jpeg");
            }
            return NotFound();
        }

        // Vulnerable: Command Injection
        [HttpPost("generate-report")]
        public IActionResult GenerateReport(string format)
        {
            // Vulnerable: Command injection through format parameter
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "report-generator.exe";
            process.StartInfo.Arguments = $"--format {format}";
            process.Start();
            return Ok("Report generation started");
        }

        // Vulnerable: Insecure Deserialization
        [HttpPost("update-product")]
        public IActionResult UpdateProduct([FromBody] string productJson)
        {
            // Vulnerable: Unsafe deserialization
            var product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(productJson);
            _context.Products.Update(product);
            _context.SaveChanges();
            return Ok(product);
        }

        // Vulnerable: IDOR
        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            // Vulnerable: No authorization check
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // Vulnerable: XSS
        [HttpPost("review")]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            // Vulnerable: No input sanitization
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }

        // Vulnerable: Race Condition
        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseProduct(int productId, int quantity)
        {
            // Vulnerable: No transaction or locking
            var product = await _context.Products.FindAsync(productId);
            if (product.Stock >= quantity)
            {
                product.Stock -= quantity;
                await _context.SaveChangesAsync();
                return Ok("Purchase successful");
            }
            return BadRequest("Insufficient stock");
        }

        // Vulnerable: Insecure Direct Object Reference
        [HttpPut("update-price/{id}")]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            // Vulnerable: No authorization check
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            
            product.Price = newPrice;
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // Vulnerable: Information Disclosure
        [HttpGet("error-details")]
        public IActionResult GetErrorDetails(string errorId)
        {
            // Vulnerable: Exposing internal error details
            try
            {
                throw new Exception("Internal error details");
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    ErrorId = errorId,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    InnerException = ex.InnerException?.Message
                });
            }
        }

        // Vulnerable: SQL Injection via POST
        [HttpPost("sqli-post")]
        public async Task<IActionResult> SqliPost([FromBody] dynamic body)
        {
            string name = body.name;
            string sql = $"SELECT * FROM Products WHERE Name = '{name}'";
            var products = await _context.Products.FromSqlRaw(sql).ToListAsync();
            return Ok(products);
        }

        // Vulnerable: Stored XSS in product metadata
        [HttpPost("add-metadata/{id}")]
        public async Task<IActionResult> AddMetadata(int id, [FromBody] string metadata)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            product.Metadata = metadata; // No sanitization
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // Vulnerable: SSRF
        [HttpPost("ssrf")]
        public async Task<IActionResult> SSRF([FromBody] string url)
        {
            using var client = new HttpClient();
            var result = await client.GetStringAsync(url); // No validation
            return Ok(result);
        }

        // Vulnerable: Relative Path Traversal
        [HttpGet("relative-path")] 
        public IActionResult RelativePath(string relPath)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var fullPath = Path.Combine(basePath, relPath); // No validation
            if (System.IO.File.Exists(fullPath))
                return PhysicalFile(fullPath, "application/octet-stream");
            return NotFound();
        }

        // Vulnerable: Open Redirect
        [HttpGet("redirect")]
        public IActionResult OpenRedirect(string url)
        {
            return Redirect(url); // No validation
        }
    }
} 