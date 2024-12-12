using GA360.Domain.Core.Models;

namespace GA360.Domain.Core.Interfaces;

public interface IDashboardService
{
    Task<DashboardModel> GetRegistrations();
    Task<DashboardModel> GetNewLearners();
    Task<DashboardModel> GetActiveLearners();
    Task<DashboardModel> GetCompletedLearners();
    Task<List<DashboardModel>> GetAllStats();
    Task<List<IndustriesModel>> GetIndustryPercentageAsync(string email);
}
