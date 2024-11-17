using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificateController:ControllerBase
{
    private readonly ILogger<CourseController> _logger;
    private readonly ICertificateService _certificateService;

    public CertificateController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetCertificates()
    {
        var result = await _certificateService.GetAllCertificates();

        return Ok(result);
    }
}