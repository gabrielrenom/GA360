using GA360.Commons.Constants;
using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using GA360.Domain.Core.Services;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static GA360.Domain.Core.Interfaces.IAuditTrailService;

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
        private readonly IAuditTrailService _auditTrailService;

        private const string QualificationsCacheKey = "qualificationsCache";
        private const string QualificationsTrainingCentreCacheKey = "qualificationsTrainingCentreCache";
        private const string UserQualifications = "userqualifications";
        private const string UserDetailedQualifications = "userdetailedqualifications";


        public QualificationController(
            ILogger<QualificationController> logger,
            IQualificationService qualificationService,
            IPermissionService permissionService,
            IMemoryCache cache,
            IAuditTrailService auditTrailService)
        {
            _logger = logger;
            _qualificationService = qualificationService;
            _permissionService = permissionService;
            _cache = cache;
            _auditTrailService = auditTrailService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetQualifications()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var permissions = await _permissionService.GetPermissions(emailClaim);

            if (permissions.Role == RoleConstants.SUPER_ADMIN)
            {
                var result = await _qualificationService.GetAllQualifications();
                await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, "Retrieved all qualifications.", emailClaim);
                return Ok(result);
            }
            else if (permissions.Role == RoleConstants.TRAINING_CENTRE)
            {
                var result = await _qualificationService.GetQualificationsByTrainingCentreWithEmail(emailClaim);
                await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, "Retrieved qualifications by training centre.", emailClaim);
                return Ok(result);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("GetQualificationsWithTrainingCentres/{id?}")]
        public async Task<IActionResult> GetQualificationsWithTrainingCentres(int? id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var permissions = await _permissionService.GetPermissions(emailClaim);

            if (permissions.Role == RoleConstants.SUPER_ADMIN)
            {
                var result = await _qualificationService.GetAllQualificationsWithTrainingCentres();
                await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, "Retrieved all qualifications with training centres.", emailClaim);
                return Ok(result);
            }
            else if (permissions.Role == RoleConstants.TRAINING_CENTRE)
            {
                var result = await _qualificationService.GetAllQualificationsWithTrainingCentres(id);
                await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, $"Retrieved qualifications with training centres for training centre ID: {id}.", emailClaim);
                return Ok(result);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("GetQualificationsByUser")]
        public async Task<IActionResult> GetQualificationsByUser()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _qualificationService.GetAllQualificationsByEmail(emailClaim);
            await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, "Retrieved qualifications by user email.", emailClaim);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetQualificationsByUserId/{id}")]
        public async Task<IActionResult> GetQualificationsByUserId(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _qualificationService.GetAllQualificationsByCandidateId(id);
            await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, $"Retrieved qualifications by user ID: {id}.", emailClaim);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetQualificationsByUserId/detail/{id}")]
        public async Task<IActionResult> GetQualificationsDetailedByUserId(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _qualificationService.GetAllDetailedQualificationsByCandidateId(id);
            await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, $"Retrieved detailed qualifications by user ID: {id}.", emailClaim);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetQualificationsByTrainingId/{trainingCentreId}")]
        public async Task<IActionResult> GetQualificationsByTrainingId(int trainingCentreId)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _qualificationService.GetAllQualificationsByTrainingCentreId(trainingCentreId);
            await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, $"Retrieved qualifications by training centre ID: {trainingCentreId}.", emailClaim);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("qualificationstatuses")]
        public async Task<IActionResult> GetQualificationStatuses()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _qualificationService.GetAllQualificationsStatus();
            await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, "Retrieved qualification statuses.", emailClaim);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQualification(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var qualification = _qualificationService.GetQualification(id);
            if (qualification == null)
            {
                await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Warning, $"Qualification not found with ID: {id}.", emailClaim);
                return NotFound();
            }
            await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, $"Retrieved qualification with ID: {id}.", emailClaim);
            return Ok(qualification);
        }

        [HttpPost]
        public async Task<IActionResult> AddQualification([FromBody] QualificationWithTrainingModel qualification)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _qualificationService.AddQualification(qualification);
            await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, $"Added qualification: {result.Id}.", emailClaim);
            return CreatedAtAction(nameof(GetQualification), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQualification(int id, [FromBody] QualificationWithTrainingModel qualification)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var result = await _qualificationService.UpdateQualification(qualification);
            await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, $"Updated qualification with ID: {id}.", emailClaim);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQualification(int id)
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            var existingQualification = _qualificationService.GetQualification(id);
            if (existingQualification == null)
            {
                await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Warning, $"Qualification not found with ID: {id}.", emailClaim);
                return NotFound();
            }
            _qualificationService.DeleteQualification(id);
            _cache.Remove(QualificationsCacheKey); // Clear cache
            await _auditTrailService.InsertAudit(AuditTrailArea.Qualifications, AuditTrailType.Information, $"Deleted qualification with ID: {id}.", emailClaim);
            return NoContent();
        }
    }
}
