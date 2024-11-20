using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using GA360.DAL.Infrastructure.Contexts;
using Microsoft.Extensions.Logging;

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
           ?.Include(x => x.DocumentCustomers)
           .ThenInclude(x => x.Document)
           .Include(x => x.CustomerSkills)
           .ThenInclude(x => x.Skill)
           .Include(x => x.Address)
           .Include(x => x.QualificationCustomerCourseCertificates)
           .ThenInclude(x => x.Course)
           .Include(x => x.QualificationCustomerCourseCertificates)
           .ThenInclude(x => x.Qualification)
           .Include(x => x.QualificationCustomerCourseCertificates)
           .ThenInclude(x => x.Certificate)
           .Include(x => x.QualificationCustomerCourseCertificates)
           .ThenInclude(x => x.QualificationStatus)
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
                .Include(x=>x.TrainingCentre)
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
    }
}
