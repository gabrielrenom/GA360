using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;

namespace GA360.Domain.Core.Services;

public class CertificateService : ICertificateService
{
    private readonly ICertificateRepository _certificateRepository;

    public CertificateService(ICertificateRepository certificateRepository)
    {
        _certificateRepository = certificateRepository;
    }
    public async Task<List<Certificate>> GetAllCertificates()
    {
        return await _certificateRepository.GetAll();
    }
}
