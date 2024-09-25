using GA360.DAL.Entities.Entities;

namespace GA360.DAL.Infrastructure.Interfaces;

public interface ITrainingCentreRepository : IRepository<TrainingCentre>
{
    Task<List<TrainingCentre>> GetTrainingCentresWithAddresses();
}
