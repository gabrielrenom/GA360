using GA360.DAL.Entities.Entities;

namespace GA360.DAL.Infrastructure.Interfaces;

public interface ISkillRepository : IRepository<Skill>
{
    Task<List<CustomerSkills>> AddCustomerSkills(List<CustomerSkills> customerSkills);
    Task<bool> Remove(int customerId);
    Task<List<CustomerSkills>> UpdateCustomerSkills(List<CustomerSkills> customerSkills);
}
