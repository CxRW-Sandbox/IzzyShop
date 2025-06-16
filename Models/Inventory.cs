using System;

namespace IzzyShop.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; } // Vulnerable: No validation
        public string Notes { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Metadata { get; set; } // Vulnerable: XXE
        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; } // Vulnerable: No validation (IDOR)
    }
} 