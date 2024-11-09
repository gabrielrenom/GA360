using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Infrastructure.Repositories;
using GA360.Domain.Core.Interfaces;
using System.Linq.Expressions;

namespace GA360.Domain.Core.Services;

public class TrainingCentreService : ITrainingCentreService
{
    private readonly ITrainingCentreRepository _trainingCentreRepository;
    public TrainingCentreService(ITrainingCentreRepository trainingCentreRepository)
    {
        _trainingCentreRepository = trainingCentreRepository;
    }

    public async Task<List<TrainingCentre>> GetTrainingCentresAsync()
    {
        return await _trainingCentreRepository.GetTrainingCentresWithAddresses();
    }

    public TrainingCentre GetTrainingCentre(int id)
    {
        return _trainingCentreRepository.Get(id);
    }

    public async Task<List<TrainingCentre>> GetAllTrainingCentres()
    {
        return await _trainingCentreRepository.GetAll();
    }

    public async Task<TrainingCentre> AddTrainingCentre(TrainingCentre trainingCentre)
    {
        var result = await _trainingCentreRepository.AddAsync(trainingCentre);
        return result;
    }

    public void UpdateTrainingCentre(TrainingCentre trainingCentre)
    {
        _trainingCentreRepository.Update(trainingCentre);
        _trainingCentreRepository.SaveChanges();
    }

    public void DeleteTrainingCentre(int id)
    {
        var trainingCentre = _trainingCentreRepository.Get(id);
        if (trainingCentre != null)
        {
            _trainingCentreRepository.Delete(trainingCentre);
            _trainingCentreRepository.SaveChanges();
        }
    }
}
