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

    public CourseController(ILogger<CourseController> logger, ICourseService courseService)
    {
        _logger = logger;
        _courseService = courseService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _courseService.GetAllCourses();
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
        var result = await _courseService.AddCourse(course.ToEntity());
        return CreatedAtAction(nameof(GetCourse), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCourse(int id, [FromBody] Course course)
    {
        var existingCourse = _courseService.GetCourse(id);
        if (existingCourse == null)
        {
            return NotFound();
        }
        course.Id = id;
        _courseService.UpdateCourse(course);
        return NoContent();
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
