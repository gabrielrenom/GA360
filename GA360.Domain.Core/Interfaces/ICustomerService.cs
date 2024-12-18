using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<Customer> AddCustomer(CustomerModel customerModel);
        Task DeleteCustomer(int id);
        Task<List<Customer>> GetAll();
        Task<List<CustomerModelHighPerformance>> GetAllUltraHighPerfomance();
        Task<CustomerModel> GetCustomerByEmail(string email);
        Customer GetCustomerById(int id);
        IEnumerable<Customer> GetCustomersByCountry(int countryId);
        Task<Customer> UpdateCustomer(int id, Customer customer);
        Task<Customer> UpdateCustomer(int id, CustomerModel customer);
        Task<List<Customer>> GetAllCustomersWithEntities<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<DAL.Entities.Entities.Customer, TOrderKey>> orderBy, bool ascending = true);

        Task<List<Customer>> GetAllCustomersWithCourseQualificationRecords<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true);
        Task<object> GetBasicCustomerByEmail(string email);
        Task<bool> DeleteCustomersWithCourseQualificationRecords(int id);
        Task<CustomersWithCourseQualificationRecordsModel> UpdateCustomersWithCourseQualificationRecords(CustomersWithCourseQualificationRecordsModel customer);
        Task<CustomersWithCourseQualificationRecordsModel> CreateCustomersWithCourseQualificationRecords(CustomersWithCourseQualificationRecordsModel customer);
        Task<List<Customer>> GetAllCustomerWithCourseQualificationRecords<TOrderKey>(string email, int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true);
        Task<CustomerBatchModel> UploadBatchCandidates(CustomerBatchModel batch, string trainincCentreUser);
        Task<CustomerModel> GetCustomerByIdWithAllEntities(int id);
    }
}
