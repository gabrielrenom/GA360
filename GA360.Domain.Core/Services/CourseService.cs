using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Infrastructure.Repositories;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace GA360.Domain.Core.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<CourseService> _logger;

    public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger)
    {
        _courseRepository = courseRepository;
        _logger = logger;
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

    public async Task<CourseModel> AddCourseByTrainingId(Course course, int trainingCentreId, double? price)
    {
        var courseResult = _courseRepository.Context.Courses.Add(course);

        await _courseRepository.Context.SaveChangesAsync();

        var courseTraining = _courseRepository.Context.CourseTrainingCentre.Add(new CourseTrainingCentre
        {
            CourseId = course.Id,
            TrainingCentreId = trainingCentreId,
            Price = price
        });

        await _courseRepository.Context.SaveChangesAsync();

        var courseEntity = await _courseRepository.Context.Courses
         .Include(x => x.CourseTrainingCentres)
         .ThenInclude(x => x.TrainingCentre)
         .FirstOrDefaultAsync(x => x.Id == course.Id);

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

    public async Task<CourseModel> UpdateCourse(Course course, int? trainingCentreId, double? price)
    {
        try
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

            if (trainingCentreId != null)
            {
                var trainingcentreentity = await _courseRepository.Context.CourseTrainingCentre.FirstOrDefaultAsync(x => x.CourseId == courseentity.Id);
                if (trainingcentreentity == null)
                {
                    _courseRepository.Context.CourseTrainingCentre.Add(new CourseTrainingCentre
                    {
                        CourseId = courseentity.Id,
                        TrainingCentreId = (int)trainingCentreId,
                        Price = price
                    });
                }
                else
                {
                    trainingcentreentity.TrainingCentreId = (int)trainingCentreId;
                    trainingcentreentity.Price = price;
                    _courseRepository.Context.CourseTrainingCentre.Update(trainingcentreentity);
                }
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating course");
        }

        return null;
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

    public async Task<List<CourseModel>> GetAllCoursesWithTrainigCentresWithDetails()
    {
        var courses = await _courseRepository.Context.Courses
            .Include(x => x.CourseTrainingCentres)
            .ThenInclude(x => x.TrainingCentre)
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

    public async Task<List<CourseModel>> GetAllCoursesWithTrainigCentres()
    {
        var courses = await _courseRepository.Context.Courses
            .Include(x => x.CourseTrainingCentres)
            .ThenInclude(x => x.TrainingCentre)
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
            Sector = course.Sector,
            Price = course.CourseTrainingCentres.FirstOrDefault()?.Price
        }).ToList();

        return courseModels;
    }

    public async Task<List<CourseDetailsModel>> GetAllCoursesWithTrainigCentresAndLearners()
    {
        var courses = await _courseRepository.Context.Courses
            .Include(x => x.CourseTrainingCentres)
            .ThenInclude(x => x.TrainingCentre)
            .ToListAsync();

        var activeLearners = await _courseRepository.Context.QualificationCustomerCourseCertificates
        .Where(qcc => qcc.CourseId.HasValue)
        .GroupBy(qcc => qcc.Course.CourseTrainingCentres.FirstOrDefault().TrainingCentreId)
        .Select(group => new { TrainingCentreId = group.Key, Count = group.Count() })
        .ToListAsync();


        var courseModels = courses.Select(course => new CourseDetailsModel
        {
            Id = course.Id,
            Status = course.Status,
            Name = course.Name,
            Description = course.Description,
            Progression = 0, // Assuming this value is calculated elsewhere
            Assesor = string.Empty, // Assuming this value is populated elsewhere
            Duration = course.Duration,
            Date = course.RegistrationDate.ToString(), // Using RegistrationDate for Date field
            Card = string.Empty, // Assuming this value is populated elsewhere
            Certification = string.Empty, // Assuming this value is populated elsewhere
            TrainingCentreId = course.CourseTrainingCentres.FirstOrDefault()?.TrainingCentreId,
            TrainingCentre = course.CourseTrainingCentres.FirstOrDefault()?.TrainingCentre?.Name,
            RegistrationDate = course.RegistrationDate,
            CertificateDate = course.CertificateDate,
            ExpectedDate = course.ExpectedDate,
            CertificateNumber = course.CertificateNumber,
            Sector = course.Sector,
            Learners = activeLearners.FirstOrDefault(a => a.TrainingCentreId == course.CourseTrainingCentres.FirstOrDefault()?.TrainingCentreId)?.Count ?? 0,
            Price = course.CourseTrainingCentres.FirstOrDefault()?.Price,
        }).ToList();

        return courseModels;
    }



    public async Task<List<CourseUserModel>> GetAllCoursesByUserId(int userId)
    {
        var courses = await (from c in _courseRepository.Context.Courses
                             join qccc in _courseRepository.Context.QualificationCustomerCourseCertificates
                             on c.Id equals qccc.CourseId
                             where qccc.CustomerId == userId
                             select new CourseUserModel
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 RegistrationDate = qccc.CourseRegistrationDate,
                                 ExpectedDate = qccc.CourseExpectedDate,
                                 CertificateDate = qccc.CourseCertificateDate,
                                 CertificateNumber = qccc.CertificateNumber,
                                 Price = qccc.CoursePrice ?? 0.0
                             }).ToListAsync();

        return courses;
    }
}
