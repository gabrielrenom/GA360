using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Models;

namespace GA360.Domain.Core.Interfaces;

public interface ICourseService
{
    Course GetCourse(int id);
    Task<List<Course>> GetAllCourses();
    Task<List<CourseModel>> GetAllCoursesWithTrainigCentres();
    Task<List<CourseTrainingModel>> GetAllCoursesByTrainingCentreId(int trainingCentreId);
    Task<List<CourseUserModel>> GetAllCoursesByUserId(int userId);
    Task<Course> AddCourse(Course course);
    Task<Course> UpdateCourse(Course course);
    void DeleteCourse(int id);
    Task<Course> AddCourse(Course course, string email);
    Task<List<Course>> GetAllCoursesByTrainingId(string email);
    Task<CourseModel> AddCourseByTrainingId(Course course, int trainingCentreId, double? price);
    Task<CourseModel> UpdateCourse(Course course, int? trainingCentreId, double? price);
    Task<List<CourseDetailsModel>> GetAllCoursesWithTrainigCentresAndLearners(int? id=null);
}