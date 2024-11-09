using GA360.DAL.Entities.Entities;

namespace GA360.Domain.Core.Interfaces;

public interface ICourseService
{
    Course GetCourse(int id);
    Task<List<Course>> GetAllCourses();
    Task<Course> AddCourse(Course course);
    void UpdateCourse(Course course);
    void DeleteCourse(int id);
}