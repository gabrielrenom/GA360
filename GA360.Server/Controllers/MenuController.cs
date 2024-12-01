using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [AllowAnonymous]
        [HttpGet("landing")]
        public async Task<IActionResult> GetLandingMenu()
        {
            return Ok("Open Menu");
        }

        [Authorize]
        [HttpGet("authnlanding")]
        public async Task<IActionResult> GetAuthnMenu()
        {
            return Ok("Authn Menu");
        }
    }
}
