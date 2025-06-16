using System;
using System.Collections.Generic;

namespace IzzyShop.Models
{
    // Messaging model for user-to-user and system messages
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Subject { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Body { get; set; } // Vulnerable: No sanitization (stored XSS)
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public string Metadata { get; set; } // Vulnerable: XXE
        public string AttachmentPath { get; set; } // Vulnerable: Path traversal
        public string Status { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public string Extra3 { get; set; }
        public string Extra4 { get; set; }
        public string Extra5 { get; set; }
        public List<MessageLog> Logs { get; set; }
        // ... more properties for bloat
    }

    // Nested log class for even more bloat
    public class MessageLog
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; } // Vulnerable: No sanitization (stored XSS)
        public string Metadata { get; set; } // Vulnerable: XXE
    }
} 