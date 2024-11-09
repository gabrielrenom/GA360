using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;

namespace GA360.Domain.Core.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public Course GetCourse(int id)
    {
        return _courseRepository.Get(id);
    }

    public async Task<List<Course>> GetAllCourses()
    {
        return await _courseRepository.GetAll();
    }

    public async Task<Course> AddCourse(Course course)
    {
        var result = await _courseRepository.AddAsync(course);

        return result;
    }

    public void UpdateCourse(Course course)
    {
        _courseRepository.Update(course);
        _courseRepository.SaveChanges();
    }

    public void DeleteCourse(int id)
    {
        var course = _courseRepository.Get(id);
        if (course != null)
        {
            _courseRepository.Delete(course);
            _courseRepository.SaveChanges();
        }
    }
}
