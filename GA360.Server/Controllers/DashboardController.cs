﻿using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using GA360.Domain.Core.Models;


namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboardService;
        private readonly IMemoryCache _cache;

        public DashboardController(ILogger<DashboardController> logger, IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        [AllowAnonymous]
        [HttpGet("learnersstats")]
        public async Task<IActionResult> GetLearnersStats()
        {
            return Ok(await _dashboardService.GetAllStats()); 
        }

        [AllowAnonymous]
        [HttpGet("industriesstats")]
        public async Task<IActionResult> GetIndustriesStats()
        {
            return Ok(await _dashboardService.GetIndustryPercentageAsync());
        }
    }
}
