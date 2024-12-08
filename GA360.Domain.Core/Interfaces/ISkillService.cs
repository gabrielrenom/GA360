using GA360.DAL.Entities.Entities;

namespace GA360.Domain.Core.Interfaces;

public interface ISkillService
{
    Task<List<Skill>> GetSkills();
}
