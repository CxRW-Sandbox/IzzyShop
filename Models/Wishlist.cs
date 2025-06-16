using System.Collections.Generic;

namespace IzzyShop.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<WishlistItem> Items { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string SharedWith { get; set; } // Vulnerable: No validation (IDOR)
        public string Metadata { get; set; } // Vulnerable: XXE
        public string Notes { get; set; } // Vulnerable: No sanitization (stored XSS)
    }
    public class WishlistItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } // Vulnerable: No sanitization (stored XSS)
        public decimal Price { get; set; }
    }
} 