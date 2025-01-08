using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using GA360.DAL.Infrastructure.Contexts;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using static GA360.DAL.Entities.Enums.StatusEnum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GA360.DAL.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private ILogger<CustomerRepository> _logger;

        public CustomerRepository(CRMDbContext dbContext, ILogger<CustomerRepository> logger) : base(dbContext)
        {
            _logger = logger;
        }
        public static List<T> Pagination<T>(List<T> items, int page, int pageSize) where T : class
        {
            return items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            var result = await GetDbContext()?.Customers
               ?.Include(x => x.EthnicOrigin)
               ?.Include(x => x.Country)
               ?.Include(x => x.DocumentCustomers).ThenInclude(x => x.Document)
               ?.Include(x => x.CustomerSkills).ThenInclude(x => x.Skill)
               ?.Include(x => x.Address)
               ?.Include(x => x.QualificationCustomerCourseCertificates)
                    .ThenInclude(x => x.Course)
               ?.Include(x => x.QualificationCustomerCourseCertificates)
                    .ThenInclude(x => x.Qualification)
               ?.Include(x => x.QualificationCustomerCourseCertificates)
                    .ThenInclude(x => x.Certificate)
               ?.Include(x => x.QualificationCustomerCourseCertificates)
                    .ThenInclude(x => x.QualificationStatus)
               ?.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            return result;
        }

        public async Task<Customer> GetBasicCustomerByEmail(string email)
        {
            var result = await GetDbContext()?.Customers
                .Include(x=>x.Roles)
                .ThenInclude(x=>x.Role)
                .Include(x=>x.Address)
           .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            return result;
        }

        public async Task<Customer> GetCustomerBasicByEmail(string email)
        {
            var result = await GetDbContext()?.Customers
           ?.Include(x => x.EthnicOrigin)
           ?.Include(x => x.Country)
           .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            return result;
        }

        public IEnumerable<Customer> GetCustomersByCountry(int countryId)
        {
            return GetDbContext().Set<Customer>().Where(c => c.CountryId == countryId).ToList();
        }

        public async Task<Customer> GetWithAllEntitiesById(int id)
        {
            var result = await GetDbContext()
                .Set<Customer>()
                .Include(x => x.Address)
                .Include(x => x.Country)
                .Include(x => x.TrainingCentre)
                .Include(x => x.EthnicOrigin)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .Include(x => x.CustomerSkills)
                .ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x=>x.Id == id);

            return result;
        }

        public async Task<Customer> GetWithAllPossibleEntitiesById(int id)
        {
            var result = await GetDbContext()
                .Set<Customer>()
                .Include(x => x.Address)
                .Include(x => x.Country)
                .Include(x => x.TrainingCentre)
                .Include(x => x.EthnicOrigin)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .Include(x => x.CustomerSkills)
                .ThenInclude(x => x.Skill)
                .Include(x => x.DocumentCustomers)
                .ThenInclude(x => x.Document)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Course)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Qualification)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Certificate)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.QualificationStatus)
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<List<Customer>> GetAllCustomersWithEntities<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true)
        {
            var query = GetDbContext()
                .Set<Customer>()
                .Include(x => x.Address)
                .Include(x => x.Country)
                .Include(x => x.TrainingCentre)
                .Include(x => x.EthnicOrigin)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .Include(x => x.CustomerSkills)
                .ThenInclude(x => x.Skill)
                .Include(x=> x.DocumentCustomers)
                .ThenInclude(x=> x.Document)
                .Include(x=> x.QualificationCustomerCourseCertificates)
                .ThenInclude(x=> x.Course)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Qualification)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Certificate)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.QualificationStatus)

                .AsQueryable();

            // Apply sorting
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            // Apply pagination if pageNumber and pageSize are provided
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }


        public async Task<List<Customer>> GetAllCustomersWithEntitiesFast<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true)
        {
            var query = GetDbContext()
                           .Set<Customer>()
                           .Include(x => x.Country)
                           .Include(x => x.TrainingCentre)
                           .AsQueryable();

            // Apply sorting
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            // Apply pagination if pageNumber and pageSize are provided
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Customer>> GetAllCustomerWithCourseQualificationRecords<TOrderKey>(string email, int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true)
        {
            var query = GetDbContext()
                .Set<Customer>()
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Course)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Qualification)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Certificate)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.QualificationStatus)
                .Include(x => x.TrainingCentre)
                .Where(x=>x.Email.ToLower() == email.ToLower())
                .AsQueryable();

            // Apply sorting
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            // Apply pagination if pageNumber and pageSize are provided
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        //public async Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords(int? pageNumber, int? pageSize, string orderBy, bool ascending = true)
        //{
        //    var customers = new List<Customer>();

        //    try
        //    {
        //        // Define the base SQL query
        //        var sql = @"
        //    SELECT 
        //        c.*, 
        //        qccc.Id AS QCCCertificateId, qccc.QualificationId AS QCCQualificationId, qccc.CertificateId AS QCCCertificateId, 
        //        qccc.QualificationProgression AS QCCQualificationProgression, qccc.CourseId AS QCCCourseId, 
        //        qccc.QualificationStatusId AS QCCQualificationStatusId, qccc.CustomerId AS QCCCustomerId, 
        //        q.Name AS QualificationName, q.Status AS QualificationStatus, 
        //        q.AwardingBody AS QualificationAwardingBody, q.InternalReference AS QualificationInternalReference, 
        //        co.Id AS CourseId, co.Name AS CourseName, co.Description AS CourseDescription, 
        //        co.RegistrationDate AS CourseRegistrationDate, co.ExpectedDate AS CourseExpectedDate, 
        //        co.Duration AS CourseDuration, co.CertificateDate AS CourseCertificateDate, 
        //        co.CertificateNumber AS CourseCertificateNumber, co.Status AS CourseStatus, co.Sector AS CourseSector, 
        //        qc.Id AS CertificateId, qc.Name AS CertificateName, qc.Charge AS CertificateCharge, qc.Type AS CertificateType, 
        //        qs.Id AS QualificationStatusId, qs.Name AS QualificationStatusName, qs.Description AS QualificationStatusDescription, 
        //        tc.Id AS TrainingCentreId
        //    FROM Customers c
        //    LEFT JOIN QualificationCustomerCourseCertificates qccc ON c.Id = qccc.CustomerId
        //    LEFT JOIN Courses co ON qccc.CourseId = co.Id
        //    LEFT JOIN Qualifications q ON qccc.QualificationId = q.Id
        //    LEFT JOIN Certificates qc ON qccc.CertificateId = qc.Id
        //    LEFT JOIN QualificationStatuses qs ON qccc.QualificationStatusId = qs.Id
        //    LEFT JOIN TrainingCentres tc ON c.TrainingCentreId = tc.Id
        //";

        //        // Apply sorting
        //        sql += $" ORDER BY {orderBy} {(ascending ? "ASC" : "DESC")}";

        //        // Apply pagination if pageNumber and pageSize are provided
        //        if (pageNumber.HasValue && pageSize.HasValue)
        //        {
        //            sql += $" OFFSET {(pageNumber.Value - 1) * pageSize.Value} ROWS FETCH NEXT {pageSize.Value} ROWS ONLY";
        //        }

        //        using (var connection = new SqlConnection(GetDbContext().Database.GetConnectionString()))
        //        using (var command = new SqlCommand(sql, connection))
        //        {
        //            await connection.OpenAsync();

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    // Map Customer
        //                    var customer = new Customer
        //                    {
        //                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email")),
        //                        Role = reader.IsDBNull(reader.GetOrdinal("Role")) ? string.Empty : reader.GetString(reader.GetOrdinal("Role")),
        //                        Status = (Status)reader.GetInt32(reader.GetOrdinal("Status")),
        //                        AvatarImage = reader.IsDBNull(reader.GetOrdinal("AvatarImage")) ? string.Empty : reader.GetString(reader.GetOrdinal("AvatarImage")),
        //                        TrainingCentreId = reader.IsDBNull(reader.GetOrdinal("TrainingCentreId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("TrainingCentreId")),
        //                        QualificationCustomerCourseCertificates = new List<QualificationCustomerCourseCertificate>()
        //                    };

        //                    // Map QualificationCustomerCourseCertificate
        //                    if (!reader.IsDBNull(reader.GetOrdinal("QCCQualificationId")))
        //                    {
        //                        var qualificationCustomerCourseCertificate = new QualificationCustomerCourseCertificate
        //                        {
        //                            Id = reader.GetInt32(reader.GetOrdinal("QCCCertificateId")),
        //                            CourseId = reader.IsDBNull(reader.GetOrdinal("QCCCourseId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("QCCCourseId")),
        //                            CustomerId = reader.IsDBNull(reader.GetOrdinal("QCCCustomerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("QCCCustomerId")),
        //                            QualificationId = reader.IsDBNull(reader.GetOrdinal("QCCQualificationId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("QCCQualificationId")),
        //                            CertificateId = reader.IsDBNull(reader.GetOrdinal("QCCCertificateId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("QCCCertificateId")),
        //                            QualificationProgression = reader.GetInt32(reader.GetOrdinal("QCCQualificationProgression")),
        //                            QualificationStatusId = reader.IsDBNull(reader.GetOrdinal("QCCQualificationStatusId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("QCCQualificationStatusId")),
        //                            Course = new Course
        //                            {
        //                                Id = reader.IsDBNull(reader.GetOrdinal("CourseId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CourseId")),
        //                                Name = reader.IsDBNull(reader.GetOrdinal("CourseName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CourseName")),
        //                                Status = reader.IsDBNull(reader.GetOrdinal("CourseStatus")) ? 0 : reader.GetInt32(reader.GetOrdinal("CourseStatus")),
        //                                Sector = reader.IsDBNull(reader.GetOrdinal("CourseSector")) ? string.Empty : reader.GetString(reader.GetOrdinal("CourseSector"))
        //                            },
        //                            Qualification = new Qualification
        //                            {
        //                                Id = reader.IsDBNull(reader.GetOrdinal("QCCQualificationId")) ? 0 : reader.GetInt32(reader.GetOrdinal("QCCQualificationId")),
        //                                Name = reader.IsDBNull(reader.GetOrdinal("QualificationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("QualificationName")),
        //                                Status = reader.IsDBNull(reader.GetOrdinal("QualificationStatus")) ? 0 : reader.GetInt32(reader.GetOrdinal("QualificationStatus")),
        //                                AwardingBody = reader.IsDBNull(reader.GetOrdinal("QualificationAwardingBody")) ? string.Empty : reader.GetString(reader.GetOrdinal("QualificationAwardingBody")),
        //                                InternalReference = reader.IsDBNull(reader.GetOrdinal("QualificationInternalReference")) ? string.Empty : reader.GetString(reader.GetOrdinal("QualificationInternalReference"))
        //                            },
        //                            Certificate = new Certificate
        //                            {
        //                                Id = reader.IsDBNull(reader.GetOrdinal("QCCCertificateId")) ? 0 : reader.GetInt32(reader.GetOrdinal("QCCCertificateId")),
        //                                Name = reader.IsDBNull(reader.GetOrdinal("CertificateName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CertificateName")),
        //                                Charge = reader.IsDBNull(reader.GetOrdinal("CertificateCharge")) ? string.Empty : reader.GetString(reader.GetOrdinal("CertificateCharge")),
        //                                Type = reader.IsDBNull(reader.GetOrdinal("CertificateType")) ? string.Empty : reader.GetString(reader.GetOrdinal("CertificateType"))
        //                            },
        //                            QualificationStatus = new QualificationStatus
        //                            {
        //                                Id = reader.IsDBNull(reader.GetOrdinal("QualificationStatusId")) ? 0 : reader.GetInt32(reader.GetOrdinal("QualificationStatusId")),
        //                                Name = reader.IsDBNull(reader.GetOrdinal("QualificationStatusName")) ? string.Empty : reader.GetString(reader.GetOrdinal("QualificationStatusName")),
        //                                Description = reader.IsDBNull(reader.GetOrdinal("QualificationStatusDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("QualificationStatusDescription"))
        //                            }
        //                        };

        //                        // Add the qualificationCustomerCourseCertificate to the customer
        //                        customer.QualificationCustomerCourseCertificates.Add(qualificationCustomerCourseCertificate);
        //                    }

        //                    // Add customer to the list
        //                    customers.Add(customer);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An error occurred: {ex.Message}");
        //    }

        //    return customers;
        //}



        public async Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true)
        {
            var query = GetDbContext()
                .Set<Customer>()
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Course)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Qualification)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Certificate)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.QualificationStatus)
                .Include(x => x.TrainingCentre)
                .AsQueryable();

            // Apply sorting
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            // Apply pagination if pageNumber and pageSize are provided
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords()
        {
            var query =await GetDbContext()
                .Set<Customer>()
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Course)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Qualification)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Certificate)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.QualificationStatus)
                .Include(x => x.TrainingCentre)
                .ToListAsync();


            return query;
        }

        //public async Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true)
        //{
        //    var customers = new List<Customer>();

        //    try
        //    {
        //        // Determine the orderByColumn from the expression
        //        var orderByColumn = ((MemberExpression)orderBy.Body).Member.Name;

        //        string query = $@"
        //    WITH OrderedCustomers AS (
        //        SELECT
        //            cust.*,
        //            qccc.*,
        //            c.*,
        //            q.*,
        //            cert.*,
        //            qs.*,
        //            tc.*,
        //            ROW_NUMBER() OVER (ORDER BY 
        //                CASE WHEN @Ascending = 1 THEN cust.{orderByColumn}
        //                     ELSE NULL
        //                END ASC,
        //                CASE WHEN @Ascending = 0 THEN cust.{orderByColumn}
        //                     ELSE NULL
        //                END DESC
        //            ) AS RowNum
        //        FROM
        //            [Customers] cust
        //        LEFT JOIN
        //            [QualificationCustomerCourseCertificates] qccc ON cust.Id = qccc.CustomerId
        //        LEFT JOIN
        //            [Courses] c ON qccc.CourseId = c.Id
        //        LEFT JOIN
        //            [Qualifications] q ON qccc.QualificationId = q.Id
        //        LEFT JOIN
        //            [Certificates] cert ON qccc.CertificateId = cert.Id
        //        LEFT JOIN
        //            [QualificationStatuses] qs ON qccc.QualificationStatusId = qs.Id
        //        LEFT JOIN
        //            [TrainingCentres] tc ON cust.TrainingCentreId = tc.Id
        //    )
        //    SELECT
        //        *
        //    FROM
        //        OrderedCustomers
        //    WHERE
        //        RowNum > (@PageNumber - 1) * @PageSize
        //        AND RowNum <= @PageNumber * @PageSize
        //    ORDER BY
        //        RowNum;
        //";

        //        using (var connection = new SqlConnection(GetDbContext().Database.GetConnectionString()))
        //        {
        //            await connection.OpenAsync();

        //            using (var command = new SqlCommand(query, connection))
        //            {
        //                command.Parameters.AddWithValue("@PageNumber", pageNumber ?? 1);
        //                command.Parameters.AddWithValue("@PageSize", pageSize ?? 10);
        //                command.Parameters.AddWithValue("@Ascending", ascending ? 1 : 0);

        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    while (await reader.ReadAsync())
        //                    {
        //                        // Populate Customer object
        //                        var customer = new Customer
        //                        {
        //                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
        //                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
        //                            Contact = reader.GetString(reader.GetOrdinal("Contact")),
        //                            FatherName = reader.GetString(reader.GetOrdinal("FatherName")),
        //                            About = reader.GetString(reader.GetOrdinal("About")),
        //                            Gender = reader.GetString(reader.GetOrdinal("Gender")),
        //                            Email = reader.GetString(reader.GetOrdinal("Email")),
        //                            Role = reader.GetString(reader.GetOrdinal("Role")),
        //                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
        //                            Location = reader.GetString(reader.GetOrdinal("Location")),
        //                            Status = (Status)reader.GetInt32(reader.GetOrdinal("Status")),
        //                            CountryId = reader.GetInt32(reader.GetOrdinal("CountryId")),
        //                            DOB = reader.GetString(reader.GetOrdinal("DOB")),
        //                            NI = reader.GetString(reader.GetOrdinal("NI")),
        //                            Disability = reader.GetString(reader.GetOrdinal("Disability")),
        //                            EmploymentStatus = reader.GetString(reader.GetOrdinal("EmploymentStatus")),
        //                            Employer = reader.GetString(reader.GetOrdinal("Employer")),
        //                            ePortfolio = reader.GetString(reader.GetOrdinal("ePortfolio")),
        //                            Industry = reader.GetString(reader.GetOrdinal("Industry")),
        //                            EthnicOriginId = reader.GetInt32(reader.GetOrdinal("EthnicOriginId")),
        //                            AvatarImage = reader.IsDBNull(reader.GetOrdinal("AvatarImage")) ? null : reader.GetString(reader.GetOrdinal("AvatarImage")),
        //                            TrainingCentreId = reader.IsDBNull(reader.GetOrdinal("TrainingCentreId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("TrainingCentreId")),
        //                            QualificationCustomerCourseCertificates = new List<QualificationCustomerCourseCertificate>()
        //                        };

        //                        // Populate nested QualificationCustomerCourseCertificate object
        //                        if (!reader.IsDBNull(reader.GetOrdinal("QualificationCustomerCourseCertificateId")))
        //                        {
        //                            var qccc = new QualificationCustomerCourseCertificate
        //                            {
        //                                Id = reader.GetInt32(reader.GetOrdinal("QualificationCustomerCourseCertificateId")),
        //                                CourseId = reader.IsDBNull(reader.GetOrdinal("CourseId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("CourseId")),
        //                                CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("CustomerId")),
        //                                QualificationId = reader.IsDBNull(reader.GetOrdinal("QualificationId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("QualificationId")),
        //                                CertificateId = reader.IsDBNull(reader.GetOrdinal("CertificateId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("CertificateId")),
        //                                QualificationProgression = reader.GetInt32(reader.GetOrdinal("QualificationProgression")),
        //                                CourseProgression = reader.GetInt32(reader.GetOrdinal("CourseProgression")),
        //                                QualificationStatusId = reader.IsDBNull(reader.GetOrdinal("QualificationStatusId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("QualificationStatusId")),
        //                                Assesor = reader.GetString(reader.GetOrdinal("Assesor")),
        //                                Course = new Course
        //                                {
        //                                    Id = reader.GetInt32(reader.GetOrdinal("CourseId")),
        //                                    Name = reader.GetString(reader.GetOrdinal("CourseName")),
        //                                    Description = reader.GetString(reader.GetOrdinal("CourseDescription")),
        //                                    RegistrationDate = reader.GetDateTime(reader.GetOrdinal("CourseRegistrationDate")),
        //                                    ExpectedDate = reader.GetDateTime(reader.GetOrdinal("CourseExpectedDate")),
        //                                    Duration = reader.GetInt32(reader.GetOrdinal("CourseDuration")),
        //                                    CertificateDate = reader.GetDateTime(reader.GetOrdinal("CourseCertificateDate")),
        //                                    CertificateNumber = reader.GetString(reader.GetOrdinal("CourseCertificateNumber")),
        //                                    Status = reader.GetInt32(reader.GetOrdinal("CourseStatus")),
        //                                    Sector = reader.GetString(reader.GetOrdinal("CourseSector"))
        //                                },
        //                                Qualification = new Qualification
        //                                {
        //                                    Id = reader.GetInt32(reader.GetOrdinal("QualificationId")),
        //                                    Name = reader.GetString(reader.GetOrdinal("QualificationName")),
        //                                    RegistrationDate = reader.GetDateTime(reader.GetOrdinal("QualificationRegistrationDate")),
        //                                    ExpectedDate = reader.GetDateTime(reader.GetOrdinal("QualificationExpectedDate")),
        //                                    CertificateDate = reader.GetDateTime(reader.GetOrdinal("QualificationCertificateDate")),
        //                                    CertificateNumber = reader.GetInt32(reader.GetOrdinal("QualificationCertificateNumber")),
        //                                    Status = reader.GetInt32(reader.GetOrdinal("QualificationStatus"))
        //                                },
        //                                Certificate = new Certificate
        //                                {
        //                                    Id = reader.GetInt32(reader.GetOrdinal("CertificateId")),
        //                                    Name = reader.GetString(reader.GetOrdinal("CertificateName")),
        //                                    Charge = reader.GetString(reader.GetOrdinal("CertificateCharge")),
        //                                    Type = reader.GetString(reader.GetOrdinal("CertificateType"))
        //                                },
        //                                QualificationStatus = new QualificationStatus
        //                                {
        //                                    Id = reader.GetInt32(reader.GetOrdinal("QualificationStatusId")),
        //                                    Name = reader.GetString(reader.GetOrdinal("QualificationStatusName")),
        //                                    Description = reader.GetString(reader.GetOrdinal("QualificationStatusDescription"))
        //                                }
        //                            };
        //                            customer.QualificationCustomerCourseCertificates.Add(qccc);
        //                        }

        //                        // Populate TrainingCentre object
        //                        if (!reader.IsDBNull(reader.GetOrdinal("TrainingCentreId")))
        //                        {
        //                            customer.TrainingCentre = new TrainingCentre
        //                            {
        //                                Id = reader.GetInt32(reader.GetOrdinal("TrainingCentreId")),
        //                                Name = reader.GetString(reader.GetOrdinal("TrainingCentreName")),
        //                                AddressId = reader.GetInt32(reader.GetOrdinal("TrainingCentreAddressId"))
        //                            };
        //                        }

        //                        customers.Add(customer);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message, "Error bringing qualifications");
        //        throw;
        //    }



        //    return customers;
        //}
        public async Task<Customer> GetCustomerWithCourseQualificationRecordById(int id)
        {
            var entity = await GetDbContext()
                .Set<Customer>()
                .Include(x=>x.TrainingCentre)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Course)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Qualification)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.Certificate)
                .Include(x => x.QualificationCustomerCourseCertificates)
                .ThenInclude(x => x.QualificationStatus)
                .FirstOrDefaultAsync(x => x.QualificationCustomerCourseCertificates.Any(y => y.Id == id));

            return entity;
        }


        public async Task<bool> DeleteCustomersWithCourseQualificationRecords(int id)
        {
            try
            {
                var entity = await GetDbContext().Set<QualificationCustomerCourseCertificate>().FirstOrDefaultAsync(x => x.Id == id);

                var result = GetDbContext().Remove<QualificationCustomerCourseCertificate>(entity);

                await GetDbContext().SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in DeleteCustomersWithCourseQualificationRecords()", ex);
            }

            return false;
        }

        public async Task<QualificationCustomerCourseCertificate> UpdateCustomersWithCourseQualificationRecords(QualificationCustomerCourseCertificate qualificationCustomerCourseCertificate)
        {
            var entity = await GetDbContext().Set<QualificationCustomerCourseCertificate>().FirstOrDefaultAsync(x => x.Id == qualificationCustomerCourseCertificate.Id);

            entity.ModifiedAt = DateTime.Now;
            entity.CourseId = qualificationCustomerCourseCertificate.CourseId;
            entity.QualificationId = qualificationCustomerCourseCertificate.QualificationId;
            entity.QualificationProgression = qualificationCustomerCourseCertificate.QualificationProgression;
            entity.QualificationStatusId = qualificationCustomerCourseCertificate.QualificationStatusId;
            entity.CertificateId = qualificationCustomerCourseCertificate.CertificateId;

            GetDbContext().Set<QualificationCustomerCourseCertificate>().Update(entity);

            await GetDbContext().SaveChangesAsync();

            return entity;
        }

        public async Task<QualificationCustomerCourseCertificate> CreateCustomersWithCourseQualificationRecords(QualificationCustomerCourseCertificate qualificationCustomerCourseCertificate)
        {
            try
            {
                var entity = GetDbContext().Set<QualificationCustomerCourseCertificate>().Add(qualificationCustomerCourseCertificate);

                await GetDbContext().SaveChangesAsync();

                return qualificationCustomerCourseCertificate;
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error creating customerqualificationcourse ", ex);

                return null;
            }

        }

        public async Task<List<ApplicationPermission>> GetApplicationPermissions(string email)
        {
            var entity = await GetDbContext().ApplicationPermissions
                .Include(x => x.Role)
                .ThenInclude(x => x.UserRoles)
                .ThenInclude(x => x.Customer)
                .Where(x => x.Role.UserRoles
                .Any(ur => ur.Customer.Email.ToLower() == email.ToLower()))
                .ToListAsync();

            return entity;
        }

        public Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords(int? pageNumber, int? pageSize, string orderBy, bool ascending = true)
        {
            throw new NotImplementedException();
        }
    }
}
