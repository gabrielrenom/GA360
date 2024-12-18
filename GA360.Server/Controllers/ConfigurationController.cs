using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ConfigurationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpGet("redirecturl")]
    public IActionResult GetRedirectUrl()
    {
        var redirectUrl = _configuration["ReactApp:RedirectUrl"];
        return Ok(new { RedirectUrl = redirectUrl });
    }

    [AllowAnonymous]
    [HttpGet("logouturl")]
    public IActionResult GetLogoutUrl()
    {
        var logoutUrl = _configuration["ReactApp:LogoutUrl"];
        return Ok(new { LogoutUrl = logoutUrl });
    }

    [HttpPost("sessionout")]
    public async Task<IActionResult> SessionOut()
    {
        try
        {
            // Clear the user's cookies and sign out
            var auth = new AuthenticationProperties()
            {
                ExpiresUtc = DateTime.Now.AddDays(-5)
            };
            await HttpContext.SignOutAsync(auth);

            //await HttpContext.SignOutAsync((AuthenticationProperties..DefaultCookieAuthenticationScheme);

            // Optionally clear session data
            //HttpContext.Session.Clear();

            // Clear cookies manually if necessary
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            // Redirect to the login page or return a response
            return Ok(new { message = "Logout successful" });
        }
        catch (Exception ex)
        {

            throw;
        }
       
    }

    [HttpGet("get-csrf-token")]
    public IActionResult GetCsrfToken()
    {
        var token = Guid.NewGuid().ToString();
        HttpContext.Session.SetString("CSRF-TOKEN", token);
        return Ok(new { csrfToken = token });
    }
}
