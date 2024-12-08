using GA360.DAL.Infrastructure.Contexts;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GA360.Domain.Core.Services;

public class DashboardService : IDashboardService
{
    private readonly CRMDbContext _cRMDbContext;
    public DashboardService(CRMDbContext cRMDbContext)
    {
        _cRMDbContext = cRMDbContext;
    }

    public async Task<List<IndustriesModel>> GetIndustryPercentageAsync()
    {
        var customers = await _cRMDbContext.Customers.ToListAsync();

        if (!customers.Any())
        {
            return new List<IndustriesModel>();
        }

        var totalCustomers = customers.Count();
        var industryGroups = customers
            .GroupBy(c => c.Industry)
            .Select(g => new IndustriesModel
            {
                Industry = g.Key,
                Percentage = ((double)g.Count() / totalCustomers) * 100
            })
            .ToList();

        return industryGroups;
    }

    public async Task<List<DashboardModel>> GetAllStats()
    {
        return new List<DashboardModel>
        {
            await GetCompletedLearners(),
            await GetActiveLearners(),
            await GetRegistrations(),
            await GetNewLearners(),
            await GetRegistrations(),
        };
    }

    public async Task<DashboardModel> GetActiveLearners()
    {
        var dashboardModel = new DashboardModel();
        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetActiveLearners]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            dashboardModel = new DashboardModel
                            {
                                Percentage = Convert.ToDecimal(reader.GetValue(3)),
                                StatisticType = StatisticType.ACTIVE_LEARNERS,
                                Total = Convert.ToDecimal(reader.GetValue(1)),
                                TotalYear = Convert.ToDecimal(reader.GetValue(0))
                            };
                        }
                    }
                    else
                    {
                        dashboardModel = new DashboardModel
                        {
                            Percentage = 0,
                            StatisticType = StatisticType.NEW_LEARNERS,
                            Total = 0,
                            TotalYear = 0
                        };
                    }
                }
            }
        }

        return dashboardModel;
    }

    public async Task<DashboardModel> GetTrainingCentreStats()
    {
        var dashboardModel = new DashboardModel();
        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetCompletedCustomerStats]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            dashboardModel = new DashboardModel
                            {
                                Percentage = Convert.ToDecimal(reader.GetValue(3)),
                                StatisticType = StatisticType.TRAINING_CENTRES_STATS,
                                Total = Convert.ToDecimal(reader.GetValue(2)),
                                TotalYear = Convert.ToDecimal(reader.GetValue(1))
                            };
                        }
                    }
                    else
                    {
                        dashboardModel = new DashboardModel
                        {
                            Percentage = 0,
                            StatisticType = StatisticType.NEW_LEARNERS,
                            Total = 0,
                            TotalYear = 0
                        };
                    }
                }
            }
        }

        return dashboardModel;
    }

    public async Task<DashboardModel> GetCompletedLearners()
    {
        var dashboardModel = new DashboardModel();
        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetCompletedCustomerStats]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            dashboardModel = new DashboardModel
                            {
                                Percentage = Convert.ToDecimal(reader.GetValue(2)),
                                StatisticType = StatisticType.COMPLETED_LEARNERS,
                                Total = Convert.ToDecimal(reader.GetValue(1)),
                                TotalYear = Convert.ToDecimal(reader.GetValue(0))
                            };
                        }
                    }
                    else
                    {
                        // Handle case where there are no rows
                        dashboardModel = new DashboardModel
                        {
                            Percentage = 0,
                            StatisticType = StatisticType.NEW_LEARNERS,
                            Total = 0,
                            TotalYear = 0
                        };
                    }
                }
            }
        }

        return dashboardModel;
    }

    public async Task<DashboardModel> GetRegistrations()
    {
        var dashboardModel = new DashboardModel();
        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetCustomerRegistrationStats]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            dashboardModel = new DashboardModel
                            {
                                Percentage = Convert.ToDecimal(reader.GetValue(1)),
                                StatisticType = StatisticType.CANDIDATE_REGISTRATIONS,
                                Total = Convert.ToDecimal(reader.GetValue(2)),
                                TotalYear = Convert.ToDecimal(reader.GetValue(0))
                            };
                        }
                    }
                    else
                    {
                        // Handle case where there are no rows
                        dashboardModel = new DashboardModel
                        {
                            Percentage = 0,
                            StatisticType = StatisticType.NEW_LEARNERS,
                            Total = 0,
                            TotalYear = 0
                        };
                    }
                }
            }
        }

        return dashboardModel;
    }

    public async Task<DashboardModel> GetNewLearners()
    {
        var dashboardModel = new DashboardModel();
        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetCandidateStats]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            dashboardModel = new DashboardModel
                            {
                                Percentage = Convert.ToDecimal(reader.GetValue(3)),
                                StatisticType = StatisticType.NEW_LEARNERS,
                                Total = Convert.ToDecimal(reader.GetValue(0)),
                                TotalYear = Convert.ToDecimal(reader.GetValue(1))
                            };
                        }
                    }
                    else
                    {
                        // Handle case where there are no rows
                        dashboardModel = new DashboardModel
                        {
                            Percentage = 0,
                            StatisticType = StatisticType.NEW_LEARNERS,
                            Total = 0,
                            TotalYear = 0
                        };
                    }
                }
            }
        }

        return dashboardModel;
    }

}