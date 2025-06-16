using System;
using System.Collections.Generic;

namespace IzzyShop.Models
{
    // Analytics model for tracking user and system events
    public class Analytics
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string EventType { get; set; } // e.g., "Login", "Purchase", "ViewProduct"
        public string EventData { get; set; } // JSON or XML, vulnerable to XXE
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Metadata { get; set; } // Vulnerable: XXE
        public string SourceIp { get; set; }
        public string UserAgent { get; set; }
        public string Referrer { get; set; }
        public string SessionId { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public string Extra3 { get; set; }
        public string Extra4 { get; set; }
        public string Extra5 { get; set; }
        public List<AnalyticsDetail> Details { get; set; }
        // ... more properties for bloat
    }

    // Nested detail class for even more bloat
    public class AnalyticsDetail
    {
        public int Id { get; set; }
        public int AnalyticsId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Notes { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Metadata { get; set; } // Vulnerable: XXE
    }
} 