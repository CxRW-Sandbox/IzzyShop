using System.Collections.Generic;

namespace IzzyShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string CouponCode { get; set; }
        public string SessionId { get; set; } // Vulnerable: No validation
        public string LastUpdatedBy { get; set; }
        public string ShippingAddress { get; set; } // Vulnerable: No validation
        public string BillingAddress { get; set; } // Vulnerable: No validation
        public string PaymentMethod { get; set; } // Vulnerable: No validation
        public string Status { get; set; }
        public string Metadata { get; set; } // Vulnerable: XXE
    }
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } // Vulnerable: No sanitization (stored XSS)
    }
} 