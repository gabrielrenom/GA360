using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GA360.Domain.Core.Interfaces.IAuditTrailService;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryService _countryService;
        private readonly IAuditTrailService _auditTrailService;

        public CountryController(ILogger<CountryController> logger, ICountryService countryService, IAuditTrailService auditTrailService)
        {
            _logger = logger;
            _countryService = countryService;
            _auditTrailService = auditTrailService;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetCountries()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _countryService.GetCountries();

            await _auditTrailService.InsertAudit(AuditTrailArea.Country, AuditTrailType.Information, "Retrieved list of countries.", emailClaim);

            return Ok(result);
        }
    }
}
