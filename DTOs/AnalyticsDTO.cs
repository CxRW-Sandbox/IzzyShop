using System;
using System.Collections.Generic;

namespace IzzyShop.DTOs
{
    // Data Transfer Object for Analytics
    public class AnalyticsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; }
        public string Metadata { get; set; }
        public string SourceIp { get; set; }
        public string UserAgent { get; set; }
        public string Referrer { get; set; }
        public string SessionId { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public string Extra3 { get; set; }
        public string Extra4 { get; set; }
        public string Extra5 { get; set; }
        public List<AnalyticsDetailDTO> Details { get; set; }
        // ... more properties for bloat
    }

    public class AnalyticsDetailDTO
    {
        public int Id { get; set; }
        public int AnalyticsId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Notes { get; set; }
        public string Metadata { get; set; }
    }
} 