using System;
using System.Collections.Generic;

namespace IzzyShop.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Address { get; set; } // Vulnerable: No validation
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string Status { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public string Notes { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Metadata { get; set; } // Vulnerable: XXE
        public List<ShippingEvent> Events { get; set; }
    }
    public class ShippingEvent
    {
        public int Id { get; set; }
        public int ShippingId { get; set; }
        public string EventType { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; } // Vulnerable: No sanitization (stored XSS)
    }
} 