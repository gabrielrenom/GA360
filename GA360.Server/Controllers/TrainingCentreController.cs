using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using static GA360.Domain.Core.Interfaces.IAuditTrailService;

namespace GA360.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingCentreController : ControllerBase
{
    private readonly ILogger<TrainingCentreController> _logger;
    private readonly ITrainingCentreService _trainingCentreService;
    private readonly IAuditTrailService _auditTrailService;

    public TrainingCentreController(ILogger<TrainingCentreController> logger, ITrainingCentreService trainingCentreService, IAuditTrailService auditTrailService)
    {
        _logger = logger;
        _trainingCentreService = trainingCentreService;
        _auditTrailService = auditTrailService;
    }

    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllTrainingCentres()
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        try
        {
            var result = await _trainingCentreService.GetTrainingCentresAsync();

            await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Information, "Retrieved all training centres.", emailClaim);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting training centres: {error}", ex.Message);
            await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Error, $"Error getting training centres: {ex.Message}", emailClaim);

            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTrainingCentres()
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var trainingCentres = await _trainingCentreService.GetAllTrainingCentres();

        await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Information, "Retrieved all training centres.", emailClaim);

        return Ok(trainingCentres);
    }

    [AllowAnonymous]
    [HttpGet("qualification/{id}")]
    public async Task<IActionResult> GetTrainingCentresByQualificationId(int id)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var trainingCentres = await _trainingCentreService.GetAllTrainingCentresByQualificationId(id);

        await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Information, $"Retrieved training centres by qualification ID: {id}.", emailClaim);

        return Ok(trainingCentres);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrainingCentre(int id)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var trainingCentre = _trainingCentreService.GetTrainingCentre(id);
        if (trainingCentre == null)
        {
            await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Warning, $"Training centre not found with ID: {id}.", emailClaim);
            return NotFound();
        }

        await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Information, $"Retrieved training centre with ID: {id}.", emailClaim);
        return Ok(trainingCentre);
    }

    [HttpPost]
    public async Task<IActionResult> AddTrainingCentre([FromBody] TrainingCentreViewModel trainingCentre)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var result = await _trainingCentreService.AddTrainingCentre(trainingCentre.ToEntity());

        await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Information, $"Added training centre: {result.Id}.", emailClaim);

        return CreatedAtAction(nameof(GetTrainingCentre), new { id = result.Id }, result.ToViewModel());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrainingCentre(int id, [FromBody] TrainingCentreViewModel trainingCentre)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var result = await _trainingCentreService.UpdateTrainingCentre(trainingCentre.ToEntity());

        await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Information, $"Updated training centre with ID: {id}.", emailClaim);

        return Ok(result.ToViewModel());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainingCentre(int id)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var existingTrainingCentre = _trainingCentreService.GetTrainingCentre(id);
        if (existingTrainingCentre == null)
        {
            await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Warning, $"Training centre not found with ID: {id}.", emailClaim);
            return NotFound();
        }
        _trainingCentreService.DeleteTrainingCentre(id);

        await _auditTrailService.InsertAudit(AuditTrailArea.TrainingCentre, AuditTrailType.Information, $"Deleted training centre with ID: {id}.", emailClaim);

        return NoContent();
    }
}
