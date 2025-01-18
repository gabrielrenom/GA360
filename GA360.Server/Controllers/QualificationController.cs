using GA360.Commons.Constants;
using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
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
        private const string QualificationsTrainingCentreCacheKey = "qualificationsTrainingCentreCache";
        private const string UserQualifications = "userqualifications";
        private const string UserDetailedQualifications = "userdetailedqualifications";


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

        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<IActionResult> GetQualifications()
        //{
        //    var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        //    if (!_cache.TryGetValue(QualificationsCacheKey, out List<Qualification> qualifications))
        //    {
        //        qualifications = await _qualificationService.GetAllQualifications();

        //        var cacheEntryOptions = new MemoryCacheEntryOptions()
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(30));

        //        _cache.Set(QualificationsCacheKey, qualifications, cacheEntryOptions);
        //    }

        //    var permissions = await _permissionService.GetPermissions(emailClaim);

        //    if (permissions.Role != RoleConstants.SUPER_ADMIN)
        //    {
        //        var qualificationsPermisssions = await _permissionService.FilterPermissions(emailClaim, qualifications);

        //        return Ok(qualificationsPermisssions);
        //    }

        //    return Ok(qualifications);
        //}

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetQualifications()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var permissions = await _permissionService.GetPermissions(emailClaim);

            if (permissions.Role == RoleConstants.SUPER_ADMIN)
            {
                return Ok(await _qualificationService.GetAllQualifications());
            }
            else if (permissions.Role == RoleConstants.TRAINING_CENTRE)
            {
                return Ok(await _qualificationService.GetQualificationsByTrainingCentreWithEmail(emailClaim));
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("GetQualificationsWithTrainingCentres")]
        public async Task<IActionResult> GetQualificationsWithTrainingCentres()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var qualifications = await _qualificationService.GetAllQualificationsWithTrainingCentres();

            return Ok(qualifications);
        }


        [AllowAnonymous]
        [HttpGet("GetQualificationsByUser")]
        public async Task<IActionResult> GetQualificationsByUser()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            if (!_cache.TryGetValue($"{UserQualifications}{emailClaim}", out List<Qualification> qualifications))
            {
                qualifications = await _qualificationService.GetAllQualificationsByEmail(emailClaim);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set($"{UserQualifications}{emailClaim}", qualifications, cacheEntryOptions);
            }

            return Ok(qualifications);
        }

        [AllowAnonymous]
        [HttpGet("GetQualificationsByUserId/{id}")]
        public async Task<IActionResult> GetQualificationsByUserId(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            if (!_cache.TryGetValue($"{UserQualifications}{id}", out List<Qualification> qualifications))
            {
                qualifications = await _qualificationService.GetAllQualificationsByCandidateId(id);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set($"{UserQualifications}{id}", qualifications, cacheEntryOptions);
            }

            return Ok(qualifications);
        }

        [AllowAnonymous]
        [HttpGet("GetQualificationsByUserId/detail/{id}")]
        public async Task<IActionResult> GetQualificationsDetailedByUserId(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            //if (!_cache.TryGetValue($"{UserDetailedQualifications}{id}", out List<QualificationLearnerModel> qualifications))
            //{
               var qualifications = await _qualificationService.GetAllDetailedQualificationsByCandidateId(id);

            //    var cacheEntryOptions = new MemoryCacheEntryOptions()
            //        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            //    _cache.Set($"{UserDetailedQualifications}{id}", qualifications, cacheEntryOptions);
            //}

            return Ok(qualifications);
        }

        [AllowAnonymous]
        [HttpGet("GetQualificationsByTrainingId/{trainingCentreId}")]
        public async Task<IActionResult> GetQualificationsByTrainingId(int trainingCentreId)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            if (!_cache.TryGetValue($"{QualificationsTrainingCentreCacheKey}{emailClaim}", out List<QualificationTrainingModel> qualifications))
            {
                qualifications = await _qualificationService.GetAllQualificationsByTrainingCentreId(trainingCentreId);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set($"{QualificationsTrainingCentreCacheKey}{emailClaim}", qualifications, cacheEntryOptions);
            }

            return Ok(qualifications);
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
        public async Task<IActionResult> AddQualification([FromBody] QualificationWithTrainingModel qualification)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _qualificationService.AddQualification(qualification);
            return CreatedAtAction(nameof(GetQualification), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQualification(int id, [FromBody] QualificationWithTrainingModel qualification)
        {
            var result = await _qualificationService.UpdateQualification(qualification);
            return Ok(result);
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
