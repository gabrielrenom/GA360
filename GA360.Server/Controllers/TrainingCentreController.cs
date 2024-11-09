using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingCentreController:ControllerBase
{
    private readonly ILogger<TrainingCentreController> _logger;
    private readonly ITrainingCentreService _trainingCentreService;

    public TrainingCentreController(ILogger<TrainingCentreController> logger, ITrainingCentreService trainingCentreService)
    {
        _logger = logger;
        _trainingCentreService = trainingCentreService;
    }

    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllTrainingCentres()
    {
        try
        {
            var result = await _trainingCentreService.GetTrainingCentresAsync();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting training centres: {error}", ex.Message);

            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTrainingCentres()
    {
        var trainingCentres = await _trainingCentreService.GetAllTrainingCentres();
        return Ok(trainingCentres);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult GetTrainingCentre(int id)
    {
        var trainingCentre = _trainingCentreService.GetTrainingCentre(id);
        if (trainingCentre == null)
        {
            return NotFound();
        }
        return Ok(trainingCentre);
    }

    [HttpPost]
    public async Task<IActionResult> AddTrainingCentre([FromBody] TrainingCentreViewModel trainingCentre)
    {
        var result = await _trainingCentreService.AddTrainingCentre(trainingCentre.ToEntity());
        return CreatedAtAction(nameof(GetTrainingCentre), new { id = result.Id }, result.ToViewModel());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTrainingCentre(int id, [FromBody] TrainingCentre trainingCentre)
    {
        var existingTrainingCentre = _trainingCentreService.GetTrainingCentre(id);
        if (existingTrainingCentre == null)
        {
            return NotFound();
        }
        trainingCentre.Id = id;
        _trainingCentreService.UpdateTrainingCentre(trainingCentre);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTrainingCentre(int id)
    {
        var existingTrainingCentre = _trainingCentreService.GetTrainingCentre(id);
        if (existingTrainingCentre == null)
        {
            return NotFound();
        }
        _trainingCentreService.DeleteTrainingCentre(id);
        return NoContent();
    }
}