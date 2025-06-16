using System;

namespace IzzyShop.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } // Vulnerable: No validation
        public string ContactEmail { get; set; } // Vulnerable: No validation
        public string Phone { get; set; }
        public string Address { get; set; } // Vulnerable: No validation
        public string Notes { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Metadata { get; set; } // Vulnerable: XXE
        public string BankAccount { get; set; } // Vulnerable: Sensitive data exposure
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 