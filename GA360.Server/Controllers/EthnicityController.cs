using GA360.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EthnicityController : ControllerBase
    {
        private readonly ILogger<EthnicityController> _logger;
        private readonly IEthnicityService _ethnicityService;
        public EthnicityController(ILogger<EthnicityController> logger, IEthnicityService ethnicityService)
        {
            _logger = logger;
            _ethnicityService = ethnicityService;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllContacts()
        {
            var result = await _ethnicityService.GetAll();

            return Ok(result);
        }
    }
}
