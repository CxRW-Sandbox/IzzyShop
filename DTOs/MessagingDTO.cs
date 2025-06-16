using System;
using System.Collections.Generic;

namespace IzzyShop.DTOs
{
    // Data Transfer Object for Messaging
    public class MessagingDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public string Metadata { get; set; }
        public string AttachmentPath { get; set; }
        public string Status { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public string Extra3 { get; set; }
        public string Extra4 { get; set; }
        public string Extra5 { get; set; }
        public List<MessageLogDTO> Logs { get; set; }
        // ... more properties for bloat
    }

    public class MessageLogDTO
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; }
        public string Metadata { get; set; }
    }
} 