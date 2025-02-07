using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using GA360.Domain.Core.Models;
using static GA360.Domain.Core.Interfaces.IAuditTrailService;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboardService;
        private readonly IMemoryCache _cache;
        private readonly IAuditTrailService _auditTrailService;

        public DashboardController(ILogger<DashboardController> logger, IDashboardService dashboardService, IAuditTrailService auditTrailService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
            _auditTrailService = auditTrailService;
        }

        [AllowAnonymous]
        [HttpGet("learnersstats/{trainingCentreId?}")]
        public async Task<IActionResult> GetLearnersStats(int? trainingCentreId = null)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _dashboardService.GetAllStats(trainingCentreId);

            await _auditTrailService.InsertAudit(AuditTrailArea.Dashboard, AuditTrailType.Information, "Retrieved learners stats.", emailClaim);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("industriesstats")]
        public async Task<IActionResult> GetIndustriesStats()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _dashboardService.GetIndustryPercentageAsync(emailClaim);

            await _auditTrailService.InsertAudit(AuditTrailArea.Dashboard, AuditTrailType.Information, "Retrieved industries stats.", emailClaim);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("industriesstatsbytrainingcentreid/{id}")]
        public async Task<IActionResult> GetIndustriesStatsByTrainingCentreId(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _dashboardService.GetIndustryPercentageByTrainingCentreIdAsync(id);

            await _auditTrailService.InsertAudit(AuditTrailArea.Dashboard, AuditTrailType.Information, "Retrieved industries stats.", emailClaim);

            return Ok(result);
        }
        
    }
}
