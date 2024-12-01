using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

        var courses = await _courseService.GetAllCourses();

        var coursesPermissions = await _permissionService.FilterPermissions(emailClaim, courses);

        return Ok(coursesPermissions);
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
        var result = await _courseService.AddCourse(course.ToEntity());
        return CreatedAtAction(nameof(GetCourse), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseViewModel course)
    {
        var result = await _courseService.UpdateCourse(course.ToEntity());

        return Ok(result.ToViewModel());
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
