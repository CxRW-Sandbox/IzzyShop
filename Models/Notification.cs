using System;

namespace IzzyShop.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Channel { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Metadata { get; set; } // Vulnerable: XXE
        public string Link { get; set; } // Vulnerable: Open Redirect
    }
} 