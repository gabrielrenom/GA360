using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GA360.DAL.Infrastructure.Repositories;

public class TrainingCentreRepository : Repository<TrainingCentre>, ITrainingCentreRepository
{
    public TrainingCentreRepository(CRMDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<TrainingCentre>> GetTrainingCentresWithAddresses()
    {
        var result = await _dbContext.Set<TrainingCentre>().Include(x=>x.Address).ToListAsync();

        return result;
    }
}
