using GA360.DAL.Entities.Entities;

namespace GA360.Domain.Core.Interfaces;

public interface IQualificationService
{
    Qualification GetQualification(int id);
    Task<List<Qualification>> GetAllQualifications();
    Task<Qualification> AddQualification(Qualification qualification);
    Task<Qualification> UpdateQualification(Qualification qualification);
    void DeleteQualification(int id);
}
