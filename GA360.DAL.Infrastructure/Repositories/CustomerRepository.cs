using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace GA360.DAL.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Customer GetCustomerByEmail(string email)
        {
            return GetDbContext().Set<Customer>().FirstOrDefault(c => c.Email == email);
        }

        public IEnumerable<Customer> GetCustomersByCountry(int countryId)
        {
            return GetDbContext().Set<Customer>().Where(c => c.CountryId == countryId).ToList();
        }

    }


}
