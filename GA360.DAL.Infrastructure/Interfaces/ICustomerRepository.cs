﻿using GA360.DAL.Entities.BaseEntities;
using GA360.DAL.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Infrastructure.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer GetCustomerByEmail(string email);
        IEnumerable<Customer> GetCustomersByCountry(int countryId);
    }
}