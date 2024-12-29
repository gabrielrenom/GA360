using GA360.DAL.Entities.BaseEntities;
using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Infrastructure.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<List<ApplicationPermission>> GetApplicationPermissions(string email);
        IEnumerable<Customer> GetCustomersByCountry(int countryId);
        Task<List<Customer>> GetAllCustomersWithEntities<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true);
        Task<List<Customer>> GetAllCustomersWithEntitiesFast<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true);
        Task<Customer> GetWithAllEntitiesById(int id);
        Task<Customer> GetCustomerByEmail(string email);
        Task<Customer> GetCustomerBasicByEmail(string email);
        //Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true);
        Task<bool> DeleteCustomersWithCourseQualificationRecords(int id);
        Task<QualificationCustomerCourseCertificate> UpdateCustomersWithCourseQualificationRecords(QualificationCustomerCourseCertificate qualificationCustomerCourseCertificate);
        Task<QualificationCustomerCourseCertificate> CreateCustomersWithCourseQualificationRecords(QualificationCustomerCourseCertificate qualificationCustomerCourseCertificate);
        Task<Customer> GetCustomerWithCourseQualificationRecordById(int id);
        Task<Customer> GetBasicCustomerByEmail(string email);
        Task<List<Customer>> GetAllCustomerWithCourseQualificationRecords<TOrderKey>(string email, int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true);
        Task<Customer> GetWithAllPossibleEntitiesById(int id);
        CRMDbContext GetDbContext();
        Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords(int? pageNumber, int? pageSize, string orderBy, bool ascending = true);
        Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords();
    }
}
