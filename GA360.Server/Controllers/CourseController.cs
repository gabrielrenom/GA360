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

namespace GA360.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ILogger<CourseController> _logger;
    private readonly ICourseService _courseService;
    private readonly IPermissionService _permissionService;
    private readonly IAuditTrailService _auditTrailService;

    public CourseController(ILogger<CourseController> logger, ICourseService courseService, IPermissionService permissionService, IAuditTrailService auditTrailService)
    {
        _logger = logger;
        _courseService = courseService;
        _permissionService = permissionService;
        _auditTrailService = auditTrailService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = new List<Course>();

        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var permissions = await _permissionService.GetPermissions(emailClaim);

        if (permissions.Role == RoleConstants.SUPER_ADMIN)
        {
            return Ok(await _courseService.GetAllCoursesWithTrainigCentres());
        }
        else if (permissions.Role == RoleConstants.TRAINING_CENTRE)
        {
            courses = await _courseService.GetAllCoursesByTrainingId(emailClaim);
        }
        await _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Information, "Retrieved courses.", emailClaim);


        return Ok(courses);
    }

    [AllowAnonymous]
    [HttpGet("details/{id?}")]
    public async Task<IActionResult> GetCoursesWithDetails(int? id)
    {
        var courses = new List<Course>();

        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var permissions = await _permissionService.GetPermissions(emailClaim);

        if (permissions.Role == RoleConstants.SUPER_ADMIN)
        {
            return Ok(await _courseService.GetAllCoursesWithTrainigCentresAndLearners());
        }
        else if (permissions.Role == RoleConstants.TRAINING_CENTRE)
        {
            return Ok(await _courseService.GetAllCoursesWithTrainigCentresAndLearners(id));
        }
        await _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Information, "Retrieved courses with details.", emailClaim);

        return Ok(courses);
    }

    [AllowAnonymous]
    [HttpGet("GetCoursesByTrainingId/{trainingCentreId}")]
    public async Task<IActionResult> GetCoursesByTrainingId(int trainingCentreId)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var courses = await _courseService.GetAllCoursesByTrainingCentreId(trainingCentreId);

        await _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Information, $"Retrieved courses by training centre ID: {trainingCentreId}.", emailClaim);

        return Ok(courses);
    }

    [AllowAnonymous]
    [HttpGet("userid/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var courses = await _courseService.GetAllCoursesByUserId(userId);

        await _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Information, $"Retrieved courses by user ID: {userId}.", emailClaim);

        return Ok(courses);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult GetCourse(int id)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var course = _courseService.GetCourse(id);
        if (course == null)
        {
            _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Warning, $"Course not found with ID: {id}.", emailClaim);
            return NotFound();
        }

        _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Information, $"Retrieved course with ID: {id}.", emailClaim);

        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> AddCourse([FromBody] CourseViewModel course)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;
        Course courseResult = new();

        var permissions = await _permissionService.GetPermissions(emailClaim);

        if (permissions.Role == "Training Centre")
        {
            courseResult = await _courseService.AddCourse(course.ToEntity(), emailClaim);
        }
        else if (permissions.Role == "Super Admin")
        {
            if (course.TrainingCentreId == null)
            {
                courseResult = await _courseService.AddCourse(course.ToEntity());
            }
            else
            {
                var finalResult = await _courseService.AddCourseByTrainingId(course.ToEntity(), (int)course.TrainingCentreId, course.Price);
                await _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Information, $"Added course: {finalResult.Id}.", emailClaim);
                return Ok(finalResult);
            }
        }

        await _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Information, $"Added course: {courseResult.Id}.", emailClaim);

        return CreatedAtAction(nameof(GetCourse), new { id = courseResult.Id }, courseResult);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseViewModel course)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var result = await _courseService.UpdateCourse(course.ToEntity(), course.TrainingCentreId, course.Price);

        await _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Information, $"Updated course with ID: {id}.", emailClaim);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var existingCourse = _courseService.GetCourse(id);
        if (existingCourse == null)
        {
            await _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Warning, $"Course not found with ID: {id}.", emailClaim);
            return NotFound();
        }
        _courseService.DeleteCourse(id);

        await _auditTrailService.InsertAudit(AuditTrailArea.Courses, AuditTrailType.Information, $"Deleted course with ID: {id}.", emailClaim);

        return NoContent();
    }
}
