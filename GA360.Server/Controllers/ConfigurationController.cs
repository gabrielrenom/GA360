﻿using Microsoft.AspNetCore.Authorization;
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
    [HttpGet("redirect-url")]
    public IActionResult GetRedirectUrl()
    {
        var redirectUrl = _configuration["ReactApp:RedirectUrl"];
        return Ok(new { RedirectUrl = redirectUrl });
    }
}
