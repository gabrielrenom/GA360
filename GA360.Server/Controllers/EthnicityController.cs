using GA360.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GA360.Domain.Core.Interfaces.IAuditTrailService;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EthnicityController : ControllerBase
    {
        private readonly ILogger<EthnicityController> _logger;
        private readonly IEthnicityService _ethnicityService;
        private readonly IAuditTrailService _auditTrailService;

        public EthnicityController(ILogger<EthnicityController> logger, IEthnicityService ethnicityService, IAuditTrailService auditTrailService)
        {
            _logger = logger;
            _ethnicityService = ethnicityService;
            _auditTrailService = auditTrailService;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetEthnicities()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _ethnicityService.GetAll();

            await _auditTrailService.InsertAudit(AuditTrailArea.Ethnicity, AuditTrailType.Information, "Retrieved list of ethnicities.", emailClaim);

            return Ok(result);
        }
    }
}
