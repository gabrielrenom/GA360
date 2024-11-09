using GA360.DAL.Entities.Entities;

namespace GA360.Domain.Core.Interfaces;

public interface ITrainingCentreService
{
    Task<List<TrainingCentre>> GetTrainingCentresAsync();
    TrainingCentre GetTrainingCentre(int id);
    Task<List<TrainingCentre>> GetAllTrainingCentres();
    Task<TrainingCentre> AddTrainingCentre(TrainingCentre trainingCentre);    
    void UpdateTrainingCentre(TrainingCentre trainingCentre);
    void DeleteTrainingCentre(int id);
}
