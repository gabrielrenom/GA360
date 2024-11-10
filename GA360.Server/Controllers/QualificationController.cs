using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QualificationController : ControllerBase
{
    private readonly ILogger<QualificationController> _logger;
    private readonly IQualificationService _qualificationService;

    public QualificationController(ILogger<QualificationController> logger, IQualificationService qualificationService)
    {
        _logger = logger;
        _qualificationService = qualificationService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetQualifications()
    {
        var qualifications = await _qualificationService.GetAllQualifications();
        return Ok(qualifications);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult GetQualification(int id)
    {
        var qualification = _qualificationService.GetQualification(id);
        if (qualification == null)
        {
            return NotFound();
        }
        return Ok(qualification);
    }

    [HttpPost]
    public async Task<IActionResult> AddQualification([FromBody] QualificationViewModel qualification)
    {
        var result = await _qualificationService.AddQualification(qualification.ToEntity());
        return CreatedAtAction(nameof(GetQualification), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualification(int id, [FromBody] QualificationViewModel qualification)
    {
        var result = await _qualificationService.UpdateQualification(qualification.ToEntity());

        return Ok(result.ToViewModel());
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteQualification(int id)
    {
        var existingQualification = _qualificationService.GetQualification(id);
        if (existingQualification == null)
        {
            return NotFound();
        }
        _qualificationService.DeleteQualification(id);
        return NoContent();
    }
}

