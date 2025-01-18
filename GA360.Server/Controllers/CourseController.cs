using GA360.Commons.Constants;
using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using GA360.Domain.Core.Services;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GA360.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ILogger<CourseController> _logger;
    private readonly ICourseService _courseService;
    private readonly IPermissionService _permissionService;

    public CourseController(ILogger<CourseController> logger, ICourseService courseService, IPermissionService permissionService)
    {
        _logger = logger;
        _courseService = courseService;
        _permissionService = permissionService;
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

        return Ok(courses);
    }

    [AllowAnonymous]
    [HttpGet("details")]
    public async Task<IActionResult> GetCoursesWithDetails()
    {
        var courses = new List<Course>();

        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        //var permissions = await _permissionService.GetPermissions(emailClaim);

        return Ok(await _courseService.GetAllCoursesWithTrainigCentresAndLearners());

        //if (permissions.Role == RoleConstants.SUPER_ADMIN)
        //{
        //    return Ok(await _courseService.GetAllCoursesWithTrainigCentresAndLearners());
        //}
        //else if (permissions.Role == RoleConstants.TRAINING_CENTRE)
        //{
        //    courses = await _courseService.GetAllCoursesByTrainingId(emailClaim);
        //}

        //return Ok(courses);
    }

    [AllowAnonymous]
    [HttpGet("GetCoursesByTrainingId/{trainingCentreId}")]
    public async Task<IActionResult> GetCoursesByTrainingId(int trainingCentreId)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var courses = await _courseService.GetAllCoursesByTrainingCentreId(trainingCentreId);

        return Ok(courses);
    }

    [AllowAnonymous]
    [HttpGet("userid/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var courses = await _courseService.GetAllCoursesByUserId(userId);

        return Ok(courses);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult GetCourse(int id)
    {
        var course = _courseService.GetCourse(id);
        if (course == null)
        {
            return NotFound();
        }
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
                return Ok(finalResult);
            }
        }

        return CreatedAtAction(nameof(GetCourse), new { id = courseResult.Id }, courseResult);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseViewModel course)
    {
        var result = await _courseService.UpdateCourse(course.ToEntity(), course.TrainingCentreId, course.Price);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCourse(int id)
    {
        var existingCourse = _courseService.GetCourse(id);
        if (existingCourse == null)
        {
            return NotFound();
        }
        _courseService.DeleteCourse(id);
        return NoContent();
    }
}
