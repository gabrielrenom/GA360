using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GA360.DAL.Infrastructure.Repositories;

public class EthnicityRepository : Repository<EthnicOrigin>, IEthnicityRepository
{
    public EthnicityRepository(CRMDbContext dbContext) : base(dbContext)
    {
    }
}
