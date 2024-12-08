using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;
using System.Security;

namespace GA360.Domain.Core.Services;

public class QualificationService : IQualificationService
{
    private readonly IQualificationRepository _qualificationRepository;
    private readonly ICustomerRepository _customerRepository;

    public QualificationService(IQualificationRepository qualificationRepository, ICustomerRepository customerRepository)
    {
        _qualificationRepository = qualificationRepository;
        _customerRepository = customerRepository;

    }

    public Qualification GetQualification(int id)
    {
        return _qualificationRepository.Get(id);
    }

    public async Task<List<Qualification>> GetAllQualifications(string customerEmail)
    {
        // Get permissions for the customer
        var permissions = await _customerRepository.GetApplicationPermissions(customerEmail);

        // Get all qualifications
        var qualifications = await _qualificationRepository.GetAll();

        // Filter only the qualifications that are in the permissions
        var filteredQualifications = qualifications
            .Where(q => permissions.Any(p => p.QualificationId == q.Id))
            .ToList();

        return filteredQualifications;
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
