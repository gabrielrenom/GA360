using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using GA360.DAL.Infrastructure.Contexts;

namespace GA360.DAL.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CRMDbContext dbContext) : base(dbContext)
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



    }
}
