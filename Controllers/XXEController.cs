using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace IzzyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class XXEController : ControllerBase
    {
        // Vulnerable: XXE with external reference
        [HttpPost("parse")]
        public IActionResult Parse([FromBody] string xml)
        {
            var doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver(); // Allows external entity resolution
            doc.LoadXml(xml);
            return Ok(doc.InnerXml);
        }
    }
} 