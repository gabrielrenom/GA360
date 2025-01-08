using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Infrastructure.Repositories;
using GA360.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        var trainingCentre = _trainingCentreRepository.Context.TrainingCentres.Include(x=>x.Address).FirstOrDefault(x=>x.Id == id);

        return trainingCentre;
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

    public async Task<TrainingCentre> UpdateTrainingCentre(TrainingCentre trainingCentre)
    {
        var trainingcentreEntity = await _trainingCentreRepository.GetTrainingCentreByIdWithAddresses(trainingCentre.Id);

        trainingcentreEntity.Address.Number = trainingCentre.Address.Number;
        trainingcentreEntity.Address.Postcode = trainingCentre.Address.Postcode;
        trainingcentreEntity.Address.Street = trainingCentre.Address.Street;
        trainingcentreEntity.Address.City = trainingCentre.Address.City;
        trainingcentreEntity.Name = trainingCentre.Name;
        trainingcentreEntity.ModifiedAt = DateTime.UtcNow;

        var result = await _trainingCentreRepository.UpdateAsync(trainingCentre);

        return result;
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
