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

    public async Task<Qualification> AddQualification(Qualification qualification)
    {
        var result = await _qualificationRepository.AddAsync(qualification);
        return result;
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
                            RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                            ExpectedDate = reader.GetDateTime(reader.GetOrdinal("ExpectedDate")),
                            CertificateDate = reader.GetDateTime(reader.GetOrdinal("CertificateDate")),
                            CertificateNumber = reader.GetInt32(reader.GetOrdinal("CertificateNumber")),
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

}
