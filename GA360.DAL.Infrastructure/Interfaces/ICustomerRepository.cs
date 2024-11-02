using GA360.DAL.Entities.BaseEntities;
using GA360.DAL.Entities.Entities;
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
        IEnumerable<Customer> GetCustomersByCountry(int countryId);
        Task<List<Customer>> GetAllCustomersWithEntities<TOrderKey>(int? pageNumber, int? pageSize, Expression<Func<Customer, TOrderKey>> orderBy, bool ascending = true);
        Task<Customer> GetWithAllEntitiesById(int id);
        Task<Customer> GetCustomerByEmail(string email);
        Task<Customer> GetCustomerBasicByEmail(string email);
    }
}
