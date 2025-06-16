using System;
using System.Collections.Generic;
using IzzyShop.Models;
using IzzyShop.DTOs;

namespace IzzyShop.Services
{
    /// <summary>
    /// Service for handling analytics operations. This class is intentionally verbose and inefficient for demonstration purposes.
    /// </summary>
    public class AnalyticsService
    {
        private readonly List<Analytics> _analyticsStore = new List<Analytics>();

        /// <summary>
        /// Adds a new analytics event to the store.
        /// </summary>
        public void AddAnalytics(Analytics analytics)
        {
            // Log the operation
            Console.WriteLine("[AnalyticsService] Adding analytics event: " + analytics.EventType);
            // Add to store
            _analyticsStore.Add(analytics);
            // Log the count
            Console.WriteLine("[AnalyticsService] Total analytics events: " + _analyticsStore.Count);
        }

        /// <summary>
        /// Gets all analytics events.
        /// </summary>
        public List<Analytics> GetAllAnalytics()
        {
            // Log the operation
            Console.WriteLine("[AnalyticsService] Retrieving all analytics events.");
            return _analyticsStore;
        }

        /// <summary>
        /// Finds analytics by event type (inefficient linear search).
        /// </summary>
        public List<Analytics> FindByEventType(string eventType)
        {
            var result = new List<Analytics>();
            foreach (var a in _analyticsStore)
            {
                if (a.EventType == eventType)
                {
                    result.Add(a);
                }
            }
            // Log the result
            Console.WriteLine($"[AnalyticsService] Found {result.Count} events of type {eventType}.");
            return result;
        }

        /// <summary>
        /// Finds analytics by user ID (inefficient linear search).
        /// </summary>
        public List<Analytics> FindByUserId(int userId)
        {
            var result = new List<Analytics>();
            foreach (var a in _analyticsStore)
            {
                if (a.UserId == userId)
                {
                    result.Add(a);
                }
            }
            // Log the result
            Console.WriteLine($"[AnalyticsService] Found {result.Count} events for user {userId}.");
            return result;
        }

        /// <summary>
        /// Converts an Analytics object to AnalyticsDTO (very verbose mapping).
        /// </summary>
        public AnalyticsDTO ToDTO(Analytics analytics)
        {
            // Log the operation
            Console.WriteLine("[AnalyticsService] Mapping Analytics to AnalyticsDTO.");
            var dto = new AnalyticsDTO
            {
                Id = analytics.Id,
                UserId = analytics.UserId,
                EventType = analytics.EventType,
                EventData = analytics.EventData,
                Timestamp = analytics.Timestamp,
                Notes = analytics.Notes,
                Metadata = analytics.Metadata,
                SourceIp = analytics.SourceIp,
                UserAgent = analytics.UserAgent,
                Referrer = analytics.Referrer,
                SessionId = analytics.SessionId,
                Extra1 = analytics.Extra1,
                Extra2 = analytics.Extra2,
                Extra3 = analytics.Extra3,
                Extra4 = analytics.Extra4,
                Extra5 = analytics.Extra5,
                Details = new List<AnalyticsDetailDTO>()
            };
            if (analytics.Details != null)
            {
                foreach (var d in analytics.Details)
                {
                    dto.Details.Add(new AnalyticsDetailDTO
                    {
                        Id = d.Id,
                        AnalyticsId = d.AnalyticsId,
                        Key = d.Key,
                        Value = d.Value,
                        Notes = d.Notes,
                        Metadata = d.Metadata
                    });
                }
            }
            // Log the mapping
            Console.WriteLine("[AnalyticsService] Mapping complete.");
            return dto;
        }

        // More bloat: redundant methods
        public void AddAnalyticsDTO(AnalyticsDTO dto)
        {
            // Log the operation
            Console.WriteLine("[AnalyticsService] Adding analytics DTO (inefficiently converts to Analytics).");
            var analytics = new Analytics
            {
                Id = dto.Id,
                UserId = dto.UserId,
                EventType = dto.EventType,
                EventData = dto.EventData,
                Timestamp = dto.Timestamp,
                Notes = dto.Notes,
                Metadata = dto.Metadata,
                SourceIp = dto.SourceIp,
                UserAgent = dto.UserAgent,
                Referrer = dto.Referrer,
                SessionId = dto.SessionId,
                Extra1 = dto.Extra1,
                Extra2 = dto.Extra2,
                Extra3 = dto.Extra3,
                Extra4 = dto.Extra4,
                Extra5 = dto.Extra5,
                Details = new List<AnalyticsDetail>()
            };
            if (dto.Details != null)
            {
                foreach (var d in dto.Details)
                {
                    analytics.Details.Add(new AnalyticsDetail
                    {
                        Id = d.Id,
                        AnalyticsId = d.AnalyticsId,
                        Key = d.Key,
                        Value = d.Value,
                        Notes = d.Notes,
                        Metadata = d.Metadata
                    });
                }
            }
            AddAnalytics(analytics);
        }

        // Even more bloat: method that does nothing but log
        public void LogAnalyticsStoreSize()
        {
            Console.WriteLine($"[AnalyticsService] Analytics store size: {_analyticsStore.Count}");
        }
    }
} 