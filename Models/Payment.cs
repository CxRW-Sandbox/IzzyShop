using System;

namespace IzzyShop.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } // Vulnerable: No validation
        public string CardNumber { get; set; } // Vulnerable: Sensitive data exposure
        public string CardExpiry { get; set; }
        public string CardCvv { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Metadata { get; set; } // Vulnerable: XXE
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 