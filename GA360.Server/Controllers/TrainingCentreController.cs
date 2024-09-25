using GA360.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GA360.Server.Controllers
{
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
    }
}
