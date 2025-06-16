using System;

namespace IzzyShop.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; } // Vulnerable: No validation
        public decimal Discount { get; set; }
        public DateTime Expiry { get; set; }
        public string Description { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Metadata { get; set; } // Vulnerable: XXE
        public string CreatedBy { get; set; } // Vulnerable: No validation (IDOR)
        public bool IsActive { get; set; }
    }
} 