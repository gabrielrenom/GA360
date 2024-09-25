using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Infrastructure.Repositories;
using GA360.Domain.Core.Interfaces;
using System.Linq.Expressions;

namespace GA360.Domain.Core.Services;

public class TrainingCentreService : ITrainingCentreService
{
    private readonly ITrainingCentreRepository _trainingRepository;
    public TrainingCentreService(ITrainingCentreRepository trainingRepository)
    {
        _trainingRepository = trainingRepository;
    }

    public async Task<List<TrainingCentre>> GetTrainingCentresAsync()
    {
        return await _trainingRepository.GetTrainingCentresWithAddresses();
    }
}
