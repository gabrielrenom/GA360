using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GA360.DAL.Infrastructure.Repositories;

public class SkillRepository : Repository<Skill>, ISkillRepository
{
    public SkillRepository(CRMDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<CustomerSkills>> UpdateCustomerSkills(List<CustomerSkills> customerSkills)
    {
        GetDbContext().UpdateRange(customerSkills);
        await GetDbContext().SaveChangesAsync();

        return customerSkills;
    }

    public async Task<List<CustomerSkills>> AddCustomerSkills(List<CustomerSkills> customerSkills)
    {
        GetDbContext().AddRange(customerSkills);
        await GetDbContext().SaveChangesAsync();

        return customerSkills;
    }

    public async Task<bool> Remove(int customerId)
    {
        try
        {
            var existingEntities = GetDbContext().CustomerSkills.Where(x => x.CustomerId == customerId).ToList();
            
            if (existingEntities != null)
            {
                foreach (var existingEntity in existingEntities)
                {
                    var sql = "DELETE FROM CustomerSkills WHERE Id = @id";
                    GetDbContext().Database.ExecuteSqlRaw(sql, new SqlParameter("@id", existingEntity.Id));
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
}