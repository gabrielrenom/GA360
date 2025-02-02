using GA360.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditTrailController: ControllerBase
    {
        private readonly IAuditTrailService _auditTrailService;

        public AuditTrailController(IAuditTrailService auditTrailService)
        {
            _auditTrailService = auditTrailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuditTrail()
        {
            return Ok(await _auditTrailService.GetAuditTrails());
        }
    }
}
