using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace GA360.Domain.Core.Services;

public class QualificationService : IQualificationService
{
    private readonly IQualificationRepository _qualificationRepository;
    private readonly ICustomerRepository _customerRepository;

    public QualificationService(IQualificationRepository qualificationRepository, ICustomerRepository customerRepository)
    {
        _qualificationRepository = qualificationRepository;
        _customerRepository = customerRepository;

    }

    public Qualification GetQualification(int id)
    {
        return _qualificationRepository.Get(id);
    }

    public async Task<List<Qualification>> GetAllQualifications(string customerEmail)
    {
        // Get permissions for the customer
        var permissions = await _customerRepository.GetApplicationPermissions(customerEmail);

        // Get all qualifications
        var qualifications = await _qualificationRepository.GetAll();

        // Filter only the qualifications that are in the permissions
        var filteredQualifications = qualifications
            .Where(q => permissions.Any(p => p.QualificationId == q.Id))
            .ToList();

        return filteredQualifications;
    }

    public async Task<List<Qualification>> GetAllQualifications()
    {
        return await _qualificationRepository.GetAll();
    }

    public async Task<List<QualificationWithTrainingModel>> GetAllQualificationsWithTrainingCentres()
    {
        var qualifications = await _qualificationRepository.Context.Qualifications
            .Include(x => x.QualificationTrainingCentres)
            .ThenInclude(x => x.TrainingCentre)
            .ToListAsync();

        var qualificationCustomerCounts = await _qualificationRepository.Context.QualificationCustomerCourseCertificates
            .GroupBy(qccc => qccc.QualificationId)
            .Select(g => new
            {
                QualificationId = g.Key,
                LearnersCount = g.Select(qccc => qccc.CustomerId).Distinct().Count()
            })
            .ToListAsync();

        var result = qualifications.Select(q => new QualificationWithTrainingModel
        {
            Id = q.Id,
            Name = q.Name,
            RegistrationDate = q.RegistrationDate,
            ExpectedDate = q.ExpectedDate,
            CertificateDate = q.CertificateDate,
            CertificateNumber = q.CertificateNumber,
            Status = q.Status,
            TrainingCentreId = q.QualificationTrainingCentres.Select(qtc => qtc.TrainingCentre.Id).FirstOrDefault(),
            TrainingCentre = q.QualificationTrainingCentres.Select(qtc => qtc.TrainingCentre.Name).FirstOrDefault(),
            InternalReference = q.InternalReference,
            QAN = q.QAN,
            AwardingBody = q.AwardingBody,
            Learners = qualificationCustomerCounts.FirstOrDefault(x => x.QualificationId == q.Id)?.LearnersCount ?? 0,
            Price = q.QualificationTrainingCentres.Select(qtc => qtc.Price).FirstOrDefault(),
            Sector = q.Sector
        }).ToList();

        return result;
    }



    public async Task<Qualification> AddQualification(Qualification qualification)
    {
        var result = await _qualificationRepository.AddAsync(qualification);
        return result;
    }

    public async Task<QualificationWithTrainingModel> AddQualification(QualificationWithTrainingModel qualification)
    {
        var qualificationEntity = new Qualification
        {
            Name = qualification.Name,
            RegistrationDate = qualification.RegistrationDate,
            ExpectedDate = qualification.ExpectedDate,
            CertificateDate = qualification.CertificateDate,
            CertificateNumber = qualification.CertificateNumber,
            InternalReference = qualification.InternalReference,
            Status = qualification.Status,
            QAN = qualification.QAN,
            AwardingBody = qualification.AwardingBody,
            Sector = qualification.Sector
        };

        var result = await _qualificationRepository.AddAsync(qualificationEntity);

        // Adding the training centre relationship
        var qualificationTrainingCentre = new QualificationTrainingCentre
        {
            QualificationId = result.Id,
            TrainingCentreId = (int)qualification.TrainingCentreId,
            Price = qualification.Price
        };
        _qualificationRepository.Context.QualificationTrainingCentre.Add(qualificationTrainingCentre);
        await _qualificationRepository.Context.SaveChangesAsync();

        // Get the training centre name
        var trainingCentre = await _qualificationRepository.Context.TrainingCentres.FindAsync(qualification.TrainingCentreId);
        var qualificationWithTraining = new QualificationWithTrainingModel
        {
            Id = result.Id,
            Name = result.Name,
            RegistrationDate = result.RegistrationDate,
            ExpectedDate = result.ExpectedDate,
            CertificateDate = result.CertificateDate,
            CertificateNumber = result.CertificateNumber,
            Status = result.Status,
            InternalReference = result.InternalReference,
            TrainingCentreId = qualification.TrainingCentreId,
            QAN = result.QAN,
            TrainingCentre = trainingCentre?.Name,
            Sector = result.Sector
        };

        return qualificationWithTraining;
    }


    public async Task<QualificationWithTrainingModel> UpdateQualification(QualificationWithTrainingModel qualification)
    {
        var qualificationEntity = await _qualificationRepository.Context.Qualifications.FirstOrDefaultAsync(x=>x.Id == qualification.Id);
        if (qualificationEntity == null)
        {
            throw new Exception("Qualification not found");
        }

        qualificationEntity.Name = qualification.Name;
        qualificationEntity.RegistrationDate = qualification.RegistrationDate;
        qualificationEntity.ExpectedDate = qualification.ExpectedDate;
        qualificationEntity.CertificateDate = qualification.CertificateDate;
        qualificationEntity.CertificateNumber = qualification.CertificateNumber;
        qualificationEntity.Status = qualification.Status;
        qualificationEntity.InternalReference = qualification.InternalReference;
        qualificationEntity.QAN = qualification.QAN;
        qualificationEntity.AwardingBody = qualification.AwardingBody;
        qualificationEntity.ModifiedAt = DateTime.Now;
        qualificationEntity.Sector = qualification.Sector;


        var result = await _qualificationRepository.UpdateAsync(qualificationEntity);

        // Update the training centre relationship
        var existingTrainingCentre = await _qualificationRepository.Context.QualificationTrainingCentre
            .FirstOrDefaultAsync(qtc => qtc.QualificationId == qualification.Id);

        if (existingTrainingCentre != null)
        {
            existingTrainingCentre.TrainingCentreId = (int)qualification.TrainingCentreId;
            var qualificationTrainingCentre = await _qualificationRepository.Context.QualificationTrainingCentre.FirstOrDefaultAsync(x => x.QualificationId == qualification.Id && x.TrainingCentreId == (int)qualification.TrainingCentreId);
            qualificationTrainingCentre.Price = qualification.Price;
            _qualificationRepository.Context.QualificationTrainingCentre.Update(qualificationTrainingCentre);
        }
        else
        {
            var qualificationTrainingCentre = new QualificationTrainingCentre
            {
                QualificationId = qualification.Id,
                TrainingCentreId = (int)qualification.TrainingCentreId,
                Price = qualification.Price,
                ModifiedAt = DateTime.Now
            };
            _qualificationRepository.Context.QualificationTrainingCentre.Add(qualificationTrainingCentre);
        }

        await _qualificationRepository.Context.SaveChangesAsync();

        // Get the training centre name
        var trainingCentre = await _qualificationRepository.Context.TrainingCentres.FindAsync(qualification.TrainingCentreId);
        var qualificationWithTraining = new QualificationWithTrainingModel
        {
            Id = result.Id,
            Name = result.Name,
            RegistrationDate = result.RegistrationDate,
            ExpectedDate = result.ExpectedDate,
            CertificateDate = result.CertificateDate,
            CertificateNumber = result.CertificateNumber,
            Status = result.Status,
            TrainingCentreId = qualification.TrainingCentreId,
            TrainingCentre = trainingCentre?.Name,
            InternalReference = result.InternalReference,
            QAN = result.QAN,
            Price = qualification.Price,
            Sector = qualification.Sector
        };

        return qualificationWithTraining;
    }


    public async Task<Qualification> UpdateQualification(Qualification qualification)
    {
        var result = await _qualificationRepository.UpdateAsync(qualification);

        return result;
    }

    public void DeleteQualification(int id)
    {
        var qualification = _qualificationRepository.Get(id);
        if (qualification != null)
        {
            _qualificationRepository.Delete(qualification);
            _qualificationRepository.SaveChanges();
        }
    }

    public async Task<List<QualificationStatus>> GetAllQualificationsStatus()
    {
        var result = await _qualificationRepository.GetQualificationStatusAsync();

        return result;
    }

    public async Task<List<QualificationTrainingModel>> GetAllQualificationsByTrainingCentreId(int trainingCentreId)
    {
        var qualificationTrainingModels = new List<QualificationTrainingModel>();

        using (var connection = new SqlConnection(_qualificationRepository.Context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            var query = @"
        SELECT 
            qt.TrainingCentreId,
            q.Id AS QualificationId,
            q.InternalReference,
            q.Name AS QualificationName,
            q.AwardingBody,
            SUM(CASE WHEN c.Role = 'Candidate' THEN 1 ELSE 0 END) AS Learners,
            SUM(CASE WHEN c.Role = 'Assessor' THEN 1 ELSE 0 END) AS Assessors,
            q.ExpectedDate AS ExpirationDate,
            q.Status
        FROM 
            [dbo].[QualificationTrainingCentre] qt
        JOIN 
            [dbo].[Qualifications] q ON qt.QualificationId = q.Id
        LEFT JOIN 
            [dbo].[QualificationCustomerCourseCertificates] qc ON q.Id = qc.QualificationId
        LEFT JOIN 
            [dbo].[Customers] c ON qc.CustomerId = c.Id
        WHERE 
            qt.TrainingCentreId = @TrainingCentreId
        GROUP BY 
            qt.TrainingCentreId,
            q.Id,
            q.InternalReference,
            q.Name,
            q.AwardingBody,
            q.ExpectedDate,
            q.Status
        ORDER BY 
            q.Name;";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    // Optionally, preallocate list capacity if an estimate is available
                    while (await reader.ReadAsync())
                    {
                        var qualificationModel = new QualificationTrainingModel();

                        qualificationModel.TrainingCentreId = reader.GetInt32(reader.GetOrdinal("TrainingCentreId"));
                        qualificationModel.QualificationId = reader.GetInt32(reader.GetOrdinal("QualificationId"));
                        qualificationModel.QAN = reader.GetInt32(reader.GetOrdinal("QualificationId"));
                        qualificationModel.InternalReference = reader.IsDBNull(reader.GetOrdinal("InternalReference")) ? string.Empty : reader.GetString(reader.GetOrdinal("InternalReference"));
                        qualificationModel.QualificationName = reader.IsDBNull(reader.GetOrdinal("QualificationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("QualificationName"));
                        qualificationModel.AwardingBody = reader.IsDBNull(reader.GetOrdinal("AwardingBody")) ? string.Empty : reader.GetString(reader.GetOrdinal("AwardingBody"));
                        qualificationModel.Learners = reader.GetInt32(reader.GetOrdinal("Learners"));
                        qualificationModel.Assessors = reader.GetInt32(reader.GetOrdinal("Assessors"));
                        qualificationModel.ExpirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"));
                        qualificationModel.Status = reader.GetInt32(reader.GetOrdinal("Status"));


                        qualificationTrainingModels.Add(qualificationModel);
                    }
                }
            }
        }

        return qualificationTrainingModels;
    }

    public async Task<List<Qualification>> GetQualificationsByTrainingCentreWithEmail(string email)
    {
        var qualifications = new List<Qualification>();

        using (var connection = new SqlConnection(_qualificationRepository.Context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            // Fetch TrainingCentreId using email
            var getTrainingCentreIdQuery = @"
        SELECT TrainingCentreId
        FROM [dbo].[Customers]
        WHERE Email = @Email;";

            int trainingCentreId;

            using (var command = new SqlCommand(getTrainingCentreIdQuery, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                var result = await command.ExecuteScalarAsync();
                if (result == null)
                {
                    throw new Exception("TrainingCentreId not found for the given email.");
                }

                trainingCentreId = (int)result;
            }

            // Fetch qualifications using TrainingCentreId
            var getQualificationsQuery = @"
        SELECT 
            q.Id, 
            q.Name, 
            q.RegistrationDate, 
            q.ExpectedDate, 
            q.CertificateDate, 
            q.CertificateNumber, 
            q.Status, 
            q.AwardingBody, 
            q.InternalReference
        FROM 
            [dbo].[QualificationTrainingCentre] qt
        JOIN 
            [dbo].[Qualifications] q ON qt.QualificationId = q.Id
        WHERE 
            qt.TrainingCentreId = @TrainingCentreId;";

            using (var command = new SqlCommand(getQualificationsQuery, connection))
            {
                command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var qualification = new Qualification
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                            ExpectedDate = reader.IsDBNull(reader.GetOrdinal("ExpectedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpectedDate")),
                            CertificateDate = reader.IsDBNull(reader.GetOrdinal("CertificateDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CertificateDate")),
                            CertificateNumber = reader.IsDBNull(reader.GetOrdinal("CertificateNumber")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CertificateNumber")),
                            Status = reader.GetInt32(reader.GetOrdinal("Status")),
                            AwardingBody = reader.IsDBNull(reader.GetOrdinal("AwardingBody")) ? null : reader.GetString(reader.GetOrdinal("AwardingBody")),
                            InternalReference = reader.IsDBNull(reader.GetOrdinal("InternalReference")) ? null : reader.GetString(reader.GetOrdinal("InternalReference"))
                        };

                        qualifications.Add(qualification);
                    }
                }
            }
        }

        return qualifications;
    }



    public async Task<List<Qualification>> GetAllQualificationsByEmail(string email)
    {
        var qualifications = new List<Qualification>();

        using (var connection = new SqlConnection(_qualificationRepository.Context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT 
                q.Id,
                q.Name,
                q.RegistrationDate,
                q.ExpectedDate,
                q.CertificateDate,
                q.CertificateNumber,
                q.Status,
                q.AwardingBody,
                q.InternalReference
            FROM 
                [dbo].[Qualifications] q
            JOIN 
                [dbo].[QualificationCustomerCourseCertificates] qc ON q.Id = qc.QualificationId
            JOIN 
                [dbo].[Customers] c ON qc.CustomerId = c.Id
            WHERE 
                c.Email = @Email
            ORDER BY 
                q.Name;";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var qualification = new Qualification
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                            ExpectedDate = reader.IsDBNull(reader.GetOrdinal("ExpectedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpectedDate")),
                            CertificateDate = reader.IsDBNull(reader.GetOrdinal("CertificateDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CertificateDate")),
                            CertificateNumber = reader.IsDBNull(reader.GetOrdinal("CertificateNumber")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CertificateNumber")),
                            Status = reader.GetInt32(reader.GetOrdinal("Status")),
                            AwardingBody = reader.IsDBNull(reader.GetOrdinal("AwardingBody")) ? null : reader.GetString(reader.GetOrdinal("AwardingBody")),
                            InternalReference = reader.IsDBNull(reader.GetOrdinal("InternalReference")) ? null : reader.GetString(reader.GetOrdinal("InternalReference"))
                        };

                        qualifications.Add(qualification);
                    }
                }
            }
        }

        return qualifications;
    }

    public async Task<List<Qualification>> GetAllQualificationsByCandidateId(int id)
    {
        var qualifications = new List<Qualification>();

        using (var connection = new SqlConnection(_qualificationRepository.Context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT 
                q.Id,
                q.Name,
                q.RegistrationDate,
                q.ExpectedDate,
                q.CertificateDate,
                q.CertificateNumber,
                q.Status,
                q.AwardingBody,
                q.InternalReference
            FROM 
                [dbo].[Qualifications] q
            JOIN 
                [dbo].[QualificationCustomerCourseCertificates] qc ON q.Id = qc.QualificationId
            JOIN 
                [dbo].[Customers] c ON qc.CustomerId = c.Id
            WHERE 
                c.id = @id
            ORDER BY 
                q.Name;";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var qualification = new Qualification
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                            ExpectedDate = reader.IsDBNull(reader.GetOrdinal("ExpectedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpectedDate")),
                            CertificateDate = reader.IsDBNull(reader.GetOrdinal("CertificateDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CertificateDate")),
                            CertificateNumber = reader.IsDBNull(reader.GetOrdinal("CertificateNumber")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CertificateNumber")),
                            Status = reader.GetInt32(reader.GetOrdinal("Status")),
                            AwardingBody = reader.IsDBNull(reader.GetOrdinal("AwardingBody")) ? null : reader.GetString(reader.GetOrdinal("AwardingBody")),
                            InternalReference = reader.IsDBNull(reader.GetOrdinal("InternalReference")) ? null : reader.GetString(reader.GetOrdinal("InternalReference"))
                        };

                        qualifications.Add(qualification);
                    }
                }
            }
        }

        return qualifications;
    }

    public async Task<List<QualificationLearnerModel>> GetAllDetailedQualificationsByCandidateId(int id)
    {
        var qualifications = new List<QualificationLearnerModel>();

        using (var connection = new SqlConnection(_qualificationRepository.Context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            var query = @"
                SELECT 
                    q.Id,
                    q.Name,
                    q.RegistrationDate,
                    q.ExpectedDate,
                    q.CertificateDate,
                    q.CertificateNumber,
                    q.Status,
                    q.AwardingBody,
                    q.InternalReference,
                    q.QAN,
                    qc.QualificationProgression,
                    qc.Assesor,
                    qc.QualificationPrice
                FROM 
                    [dbo].[Qualifications] q
                JOIN 
                    [dbo].[QualificationCustomerCourseCertificates] qc ON q.Id = qc.QualificationId
                JOIN 
                    [dbo].[Customers] c ON qc.CustomerId = c.Id
                WHERE 
                    c.id = @id
                ORDER BY 
                    q.Name;";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var qualification = new QualificationLearnerModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                            ExpectedDate = reader.IsDBNull(reader.GetOrdinal("ExpectedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpectedDate")),
                            CertificateDate = reader.IsDBNull(reader.GetOrdinal("CertificateDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CertificateDate")),
                            CertificateNumber = reader.IsDBNull(reader.GetOrdinal("CertificateNumber")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CertificateNumber")),
                            Status = reader.GetInt32(reader.GetOrdinal("Status")),
                            AwardingBody = reader.IsDBNull(reader.GetOrdinal("AwardingBody")) ? null : reader.GetString(reader.GetOrdinal("AwardingBody")),
                            InternalReference = reader.IsDBNull(reader.GetOrdinal("InternalReference")) ? null : reader.GetString(reader.GetOrdinal("InternalReference")),
                            QAN = reader.IsDBNull(reader.GetOrdinal("QAN")) ? null : reader.GetString(reader.GetOrdinal("QAN")),
                            Progression = reader.IsDBNull(reader.GetOrdinal("QualificationProgression")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("QualificationProgression")),
                            Assessor = reader.IsDBNull(reader.GetOrdinal("Assesor")) ? null : reader.GetString(reader.GetOrdinal("Assesor")),
                            Price = reader.IsDBNull(reader.GetOrdinal("QualificationPrice")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("QualificationPrice"))
                        };

                        qualifications.Add(qualification);
                    }
                }
            }
        }

        return qualifications;
    }



}
