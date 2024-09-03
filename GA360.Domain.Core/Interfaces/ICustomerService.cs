using GA360.DAL.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> AddCustomer(Customer customer);
        void DeleteCustomer(int id);
        Task<List<Customer>> GetAll();
        Customer GetCustomerByEmail(string email);
        Customer GetCustomerById(int id);
        IEnumerable<Customer> GetCustomersByCountry(int countryId);
        void UpdateCustomer(Customer customer);
    }
}
