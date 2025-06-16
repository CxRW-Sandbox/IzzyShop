using System.ComponentModel.DataAnnotations;

namespace IzzyShop.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; } // Vulnerable: No sanitization (stored XSS)
    }
} 