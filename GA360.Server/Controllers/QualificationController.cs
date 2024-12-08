using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QualificationController : ControllerBase
    {
        private readonly ILogger<QualificationController> _logger;
        private readonly IQualificationService _qualificationService;
        private readonly IPermissionService _permissionService;
        private readonly IMemoryCache _cache;

        private const string QualificationsCacheKey = "qualificationsCache";

        public QualificationController(
            ILogger<QualificationController> logger,
            IQualificationService qualificationService,
            IPermissionService permissionService,
            IMemoryCache cache)
        {
            _logger = logger;
            _qualificationService = qualificationService;
            _permissionService = permissionService;
            _cache = cache;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetQualifications()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            if (!_cache.TryGetValue(QualificationsCacheKey, out List<Qualification> qualifications))
            {
                qualifications = await _qualificationService.GetAllQualifications();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(QualificationsCacheKey, qualifications, cacheEntryOptions);
            }

            var qualificationsPermisssions = await _permissionService.FilterPermissions(emailClaim, qualifications);

            return Ok(qualificationsPermisssions);
        }

        [AllowAnonymous]
        [HttpGet("qualificationstatuses")]
        public async Task<IActionResult> GetQualificationStatuses()
        {
            var qualifications = await _qualificationService.GetAllQualificationsStatus();
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
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _qualificationService.AddQualification(qualification.ToEntity());
            _cache.Remove(QualificationsCacheKey); // Clear cache
            return CreatedAtAction(nameof(GetQualification), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQualification(int id, [FromBody] QualificationViewModel qualification)
        {
            var result = await _qualificationService.UpdateQualification(qualification.ToEntity());
            _cache.Remove(QualificationsCacheKey); // Clear cache
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
            _cache.Remove(QualificationsCacheKey); // Clear cache
            return NoContent();
        }
    }
}
