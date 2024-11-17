using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Infrastructure.Repositories;

public class QualificationRepository : Repository<Qualification>, IQualificationRepository
{

    public QualificationRepository(CRMDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<QualificationStatus>> GetQualificationStatusAsync()
    {
        return await GetDbContext()?.QualificationStatuses.ToListAsync();
    }
}
