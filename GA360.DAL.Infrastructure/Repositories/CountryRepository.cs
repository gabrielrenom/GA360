using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GA360.DAL.Infrastructure.Repositories;

public class CountryRepository : Repository<Country>, ICountryRepository
{
    public CountryRepository(DbContext dbContext) : base(dbContext)
    {
    }
}