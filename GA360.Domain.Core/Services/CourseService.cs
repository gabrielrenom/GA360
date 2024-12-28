using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Infrastructure.Repositories;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Course>> GetAllCoursesByTrainingId(string email)
    {
        var customer = await _courseRepository.Context.Customers.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        return await _courseRepository.Context.Courses.Where(x => x.CourseTrainingCentres.Any(x => x.TrainingCentreId == customer.TrainingCentreId)).ToListAsync();
    }

    public async Task<Course> AddCourse(Course course)
    {
        var result = await _courseRepository.AddAsync(course);

        return result;
    }

    public async Task<CourseModel> AddCourseByTrainingId(Course course, int trainingCentreId)
    {
        var courseResult = _courseRepository.Context.Courses.Add(course);

        await _courseRepository.Context.SaveChangesAsync();

        var courseTraining = _courseRepository.Context.CourseTrainingCentre.Add(new CourseTrainingCentre
        {
            CourseId = course.Id,
            TrainingCentreId = trainingCentreId
        });

        await _courseRepository.Context.SaveChangesAsync();

        var courseEntity = await _courseRepository.Context.Courses
         .Include(x => x.CourseTrainingCentres)
         .ThenInclude(x => x.TrainingCentre)
         .FirstOrDefaultAsync(x=> x.Id == course.Id);

        var courseModels = new CourseModel
        {
            Id = courseEntity.Id,
            Status = course.Status,
            Name = course.Name,
            Description = course.Description,
            Progression = 0, // Assuming this value is calculated elsewhere
            Assesor = string.Empty, // Assuming this value is populated elsewhere
            Duration = course.Duration,
            Date = course.RegistrationDate.ToString(),//course.RegistrationDate.ToString("yyyy-MM-dd"), // Using RegistrationDate for Date field
            Card = string.Empty, // Assuming this value is populated elsewhere
            Certification = string.Empty, // Assuming this value is populated elsewhere
            TrainingCentreId = courseEntity.CourseTrainingCentres.FirstOrDefault()?.TrainingCentreId,
            TrainingCentre = courseEntity.CourseTrainingCentres.FirstOrDefault()?.TrainingCentre?.Name,
            RegistrationDate = courseEntity.RegistrationDate,
            CertificateDate = courseEntity.CertificateDate,
            ExpectedDate = courseEntity.ExpectedDate,
            CertificateNumber = courseEntity.CertificateNumber

        };


        return courseModels;
    }

    public async Task<Course> AddCourse(Course course, string email)
    {
        var customer = await _courseRepository.Context.Customers.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

        if (customer.TrainingCentreId != null)
        {
            var courseResult = _courseRepository.Context.Courses.Add(course);

            await _courseRepository.Context.SaveChangesAsync();

            var courseTraining = _courseRepository.Context.CourseTrainingCentre.Add(new CourseTrainingCentre
            {
                CourseId = course.Id,
                TrainingCentreId = (int)customer.TrainingCentreId
            });

            await _courseRepository.Context.SaveChangesAsync();
        }

        return course;
    }

    public async Task<Course> UpdateCourse(Course course)
    {
        var courseentity = _courseRepository.Get(course.Id);

        courseentity.Duration = course.Duration;
        courseentity.CertificateNumber = course.CertificateNumber;
        courseentity.CertificateDate = course.CertificateDate;
        courseentity.Description = course.Description;
        courseentity.ExpectedDate = course.ExpectedDate;
        courseentity.Name = course.Name;
        courseentity.Status = course.Status;
        courseentity.RegistrationDate = course.RegistrationDate;
        courseentity.Sector = course.Sector;

        var result = await _courseRepository.UpdateAsync(courseentity);



        return result;
    }

    public async Task<CourseModel> UpdateCourse(Course course, int? trainingCentreId)
    {
        var courseentity = _courseRepository.Get(course.Id);

        courseentity.Duration = course.Duration;
        courseentity.CertificateNumber = course.CertificateNumber;
        courseentity.CertificateDate = course.CertificateDate;
        courseentity.Description = course.Description;
        courseentity.ExpectedDate = course.ExpectedDate;
        courseentity.Name = course.Name;
        courseentity.Status = course.Status;
        courseentity.RegistrationDate = course.RegistrationDate;
        courseentity.Sector = course.Sector;

        var result = await _courseRepository.UpdateAsync(courseentity);

        if (trainingCentreId!=null)
        {
            var trainingcentreentity = await _courseRepository.Context.CourseTrainingCentre.FirstOrDefaultAsync(x => x.CourseId == courseentity.Id);
            trainingcentreentity.TrainingCentreId = (int)trainingCentreId;
            _courseRepository.Context.CourseTrainingCentre.Update(trainingcentreentity);
            await _courseRepository.SaveChangesAsync();
        }

        var courseEntity = await _courseRepository.Context.Courses
         .Include(x => x.CourseTrainingCentres)
         .ThenInclude(x => x.TrainingCentre)
         .FirstOrDefaultAsync(x => x.Id == course.Id);

        var courseModel = new CourseModel
        {
            Id = courseEntity.Id,
            Status = course.Status,
            Name = course.Name,
            Description = course.Description,
            Progression = 0, // Assuming this value is calculated elsewhere
            Assesor = string.Empty, // Assuming this value is populated elsewhere
            Duration = course.Duration,
            Date = course.RegistrationDate.ToString(),//course.RegistrationDate.ToString("yyyy-MM-dd"), // Using RegistrationDate for Date field
            Card = string.Empty, // Assuming this value is populated elsewhere
            Certification = string.Empty, // Assuming this value is populated elsewhere
            TrainingCentreId = courseEntity.CourseTrainingCentres.FirstOrDefault()?.TrainingCentreId,
            TrainingCentre = courseEntity.CourseTrainingCentres.FirstOrDefault()?.TrainingCentre?.Name,
            RegistrationDate = courseEntity.RegistrationDate,
            CertificateDate = courseEntity.CertificateDate,
            ExpectedDate = courseEntity.ExpectedDate,
            CertificateNumber = courseEntity.CertificateNumber

        };

        return courseModel;
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

    public async Task<List<CourseTrainingModel>> GetAllCoursesByTrainingCentreId(int trainingCentreId)
    {
        var courseTrainingModels = new List<CourseTrainingModel>();

        using (var connection = new SqlConnection(_courseRepository.Context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            var query = @"
        SELECT 
            c.Id,
            c.Name,
            c.Description,
            c.RegistrationDate,
            c.ExpectedDate,
            c.Duration,
            c.CertificateDate,
            c.CertificateNumber,
            c.Status,
            c.Sector,
            COUNT(DISTINCT c1.Id) AS Learners
        FROM 
            [dbo].[CourseTrainingCentre] ct
        JOIN 
            [dbo].[Courses] c ON ct.CourseId = c.Id
        LEFT JOIN 
            [dbo].[QualificationCustomerCourseCertificates] qc ON c.Id = qc.CourseId
        LEFT JOIN 
            [dbo].[Customers] c1 ON qc.CustomerId = c1.Id AND c1.Role = 'Candidate'
        WHERE 
            ct.TrainingCentreId = @TrainingCentreId
        GROUP BY 
            c.Id,
            c.Name,
            c.Description,
            c.RegistrationDate,
            c.ExpectedDate,
            c.Duration,
            c.CertificateDate,
            c.CertificateNumber,
            c.Status,
            c.Sector
        ORDER BY 
            c.Name;";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var courseModel = new CourseTrainingModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                            ExpectedDate = reader.GetDateTime(reader.GetOrdinal("ExpectedDate")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            CertificateDate = reader.GetDateTime(reader.GetOrdinal("CertificateDate")),
                            CertificateNumber = reader.GetString(reader.GetOrdinal("CertificateNumber")),
                            Status = reader.GetInt32(reader.GetOrdinal("Status")),
                            Sector = reader.GetString(reader.GetOrdinal("Sector")),
                            Learners = reader.GetInt32(reader.GetOrdinal("Learners"))
                        };

                        courseTrainingModels.Add(courseModel);
                    }
                }
            }
        }

        return courseTrainingModels;
    }

    public async Task<List<CourseModel>> GetAllCoursesWithTrainigCentres()
    {
        var courses = await _courseRepository.Context.Courses
            .Include(x => x.CourseTrainingCentres)
            .ThenInclude(x=>x.TrainingCentre)
            .ToListAsync();

        var courseModels = courses.Select(course => new CourseModel
        {
            Id = course.Id,
            Status = course.Status,
            Name = course.Name,
            Description = course.Description,
            Progression = 0, // Assuming this value is calculated elsewhere
            Assesor = string.Empty, // Assuming this value is populated elsewhere
            Duration = course.Duration,
            Date = course.RegistrationDate.ToString(),//course.RegistrationDate.ToString("yyyy-MM-dd"), // Using RegistrationDate for Date field
            Card = string.Empty, // Assuming this value is populated elsewhere
            Certification = string.Empty, // Assuming this value is populated elsewhere
            TrainingCentreId = course.CourseTrainingCentres.FirstOrDefault()?.TrainingCentreId,
            TrainingCentre = course.CourseTrainingCentres.FirstOrDefault()?.TrainingCentre?.Name,
            RegistrationDate = course.RegistrationDate,
            CertificateDate = course.CertificateDate,
            ExpectedDate = course.ExpectedDate,
            CertificateNumber = course.CertificateNumber,
            Sector = course.Sector

        }).ToList();

        return courseModels;
    }

}
