using Microsoft.AspNetCore.Mvc;
using IzzyShop.Models;
using IzzyShop.DTOs;
using IzzyShop.Services;
using System;
using System.Collections.Generic;

namespace IzzyShop.Controllers
{
    /// <summary>
    /// Controller for analytics operations. This controller is intentionally verbose and inefficient for demonstration purposes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsService _service;

        /// <summary>
        /// Constructor for AnalyticsController. Initializes the AnalyticsService.
        /// </summary>
        public AnalyticsController()
        {
            // Log the initialization
            Console.WriteLine("[AnalyticsController] Initializing AnalyticsService.");
            _service = new AnalyticsService();
        }

        /// <summary>
        /// Adds a new analytics event (very verbose endpoint).
        /// </summary>
        [HttpPost("add")]
        public IActionResult AddAnalytics([FromBody] AnalyticsDTO dto)
        {
            // Log the request
            Console.WriteLine("[AnalyticsController] AddAnalytics called.");
            // Redundant null check
            if (dto == null)
            {
                Console.WriteLine("[AnalyticsController] DTO is null.");
                return BadRequest("DTO cannot be null.");
            }
            // Add analytics
            _service.AddAnalyticsDTO(dto);
            // Log the operation
            Console.WriteLine("[AnalyticsController] Analytics event added.");
            // Redundant response
            return Ok(new { message = "Analytics event added.", dto });
        }

        /// <summary>
        /// Gets all analytics events (very verbose endpoint).
        /// </summary>
        [HttpGet("all")]
        public IActionResult GetAllAnalytics()
        {
            // Log the request
            Console.WriteLine("[AnalyticsController] GetAllAnalytics called.");
            var analytics = _service.GetAllAnalytics();
            var dtos = new List<AnalyticsDTO>();
            foreach (var a in analytics)
            {
                dtos.Add(_service.ToDTO(a));
            }
            // Log the operation
            Console.WriteLine($"[AnalyticsController] Returning {dtos.Count} analytics events.");
            return Ok(dtos);
        }

        /// <summary>
        /// Finds analytics by event type (very verbose endpoint).
        /// </summary>
        [HttpGet("find-by-type")]
        public IActionResult FindByEventType([FromQuery] string eventType)
        {
            // Log the request
            Console.WriteLine($"[AnalyticsController] FindByEventType called with eventType={eventType}.");
            var analytics = _service.FindByEventType(eventType);
            var dtos = new List<AnalyticsDTO>();
            foreach (var a in analytics)
            {
                dtos.Add(_service.ToDTO(a));
            }
            // Log the operation
            Console.WriteLine($"[AnalyticsController] Returning {dtos.Count} analytics events of type {eventType}.");
            return Ok(dtos);
        }

        /// <summary>
        /// Finds analytics by user ID (very verbose endpoint).
        /// </summary>
        [HttpGet("find-by-user")]
        public IActionResult FindByUserId([FromQuery] int userId)
        {
            // Log the request
            Console.WriteLine($"[AnalyticsController] FindByUserId called with userId={userId}.");
            var analytics = _service.FindByUserId(userId);
            var dtos = new List<AnalyticsDTO>();
            foreach (var a in analytics)
            {
                dtos.Add(_service.ToDTO(a));
            }
            // Log the operation
            Console.WriteLine($"[AnalyticsController] Returning {dtos.Count} analytics events for user {userId}.");
            return Ok(dtos);
        }

        /// <summary>
        /// Logs the analytics store size (bloat endpoint).
        /// </summary>
        [HttpGet("log-size")]
        public IActionResult LogSize()
        {
            // Log the request
            Console.WriteLine("[AnalyticsController] LogSize called.");
            _service.LogAnalyticsStoreSize();
            return Ok(new { message = "Analytics store size logged." });
        }
    }
} 