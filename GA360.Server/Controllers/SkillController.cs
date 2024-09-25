using GA360.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ILogger<EthnicityController> _logger;
        private readonly ISkillService _skillService;
        public SkillController(ILogger<EthnicityController> logger, ISkillService skillService)
        {
            _logger = logger;
            _skillService = skillService;
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetSkills()
        {
            var result = await _skillService.GetSkills();

            return Ok(result);
        }
    }
}
