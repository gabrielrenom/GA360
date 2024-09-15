using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace GA360.DAL.Infrastructure.Repositories;

public class SkillRepository : Repository<Skill>, ISkillRepository
{
    public SkillRepository(DbContext dbContext) : base(dbContext)
    {
    }
}