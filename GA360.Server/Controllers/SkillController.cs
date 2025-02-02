using GA360.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GA360.Domain.Core.Interfaces.IAuditTrailService;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ILogger<SkillController> _logger;
        private readonly ISkillService _skillService;
        private readonly IAuditTrailService _auditTrailService;

        public SkillController(ILogger<SkillController> logger, ISkillService skillService, IAuditTrailService auditTrailService)
        {
            _logger = logger;
            _skillService = skillService;
            _auditTrailService = auditTrailService;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetSkills()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _skillService.GetSkills();

            await _auditTrailService.InsertAudit(AuditTrailArea.Skills, AuditTrailType.Information, "Retrieved list of skills.", emailClaim);

            return Ok(result);
        }
    }
}
