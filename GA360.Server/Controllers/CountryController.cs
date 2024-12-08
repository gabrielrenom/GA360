using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<EthnicityController> _logger;
        private readonly ICountryService _countryService;

        public CountryController(ILogger<EthnicityController> logger, ICountryService countryService)
        {
            _logger = logger;
            _countryService = countryService;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetCountries()
        {
            var result = await _countryService.GetCountries();

            return Ok(result);
        }
    }
}
