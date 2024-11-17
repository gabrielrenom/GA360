using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;

namespace GA360.Domain.Core.Services;

public class QualificationService : IQualificationService
{
    private readonly IQualificationRepository _qualificationRepository;

    public QualificationService(IQualificationRepository qualificationRepository)
    {
        _qualificationRepository = qualificationRepository;
    }

    public Qualification GetQualification(int id)
    {
        return _qualificationRepository.Get(id);
    }

    public async Task<List<Qualification>> GetAllQualifications()
    {
        return await _qualificationRepository.GetAll();
    }

    public async Task<Qualification> AddQualification(Qualification qualification)
    {
        var result = await _qualificationRepository.AddAsync(qualification);
        return result;
    }

    public async Task<Qualification> UpdateQualification(Qualification qualification)
    {
        var result = await _qualificationRepository.UpdateAsync(qualification);

        return result;
    }

    public void DeleteQualification(int id)
    {
        var qualification = _qualificationRepository.Get(id);
        if (qualification != null)
        {
            _qualificationRepository.Delete(qualification);
            _qualificationRepository.SaveChanges();
        }
    }

    public async Task<List<QualificationStatus>> GetAllQualificationsStatus()
    {
        var result = await _qualificationRepository.GetQualificationStatusAsync();

        return result;
    }
}
