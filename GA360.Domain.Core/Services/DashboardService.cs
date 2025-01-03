﻿using GA360.DAL.Infrastructure.Contexts;
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

    public async Task<List<IndustriesModel>> GetIndustryPercentageAsync(string email)
    {
        var industryList = new List<IndustriesModel>();

        using (var connection = new SqlConnection(_cRMDbContext.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            // Fetch the user's role
            string userRoleQuery = @"
            SELECT r.Name 
            FROM [dbo].[UserRoles] ur
            JOIN [dbo].[Roles] r ON ur.RoleId = r.Id
            JOIN [dbo].[Customers] c ON ur.CustomerId = c.Id
            WHERE c.Email = @Email";

            string userRole;

            using (var roleCommand = new SqlCommand(userRoleQuery, connection))
            {
                roleCommand.Parameters.AddWithValue("@Email", email);
                userRole = (string)await roleCommand.ExecuteScalarAsync();
            }

            // Build the main query
            var query = @"
            SELECT DISTINCT
                c.[Sector] AS Industry,
                COUNT(*) * 100.0 / SUM(COUNT(*)) OVER (PARTITION BY tc.Id) AS Percentage
            FROM
                [dbo].[QualificationCustomerCourseCertificates] qccc
            JOIN
                [dbo].[Courses] c ON qccc.CourseId = c.Id
            JOIN
                [dbo].[Customers] cust ON qccc.CustomerId = cust.Id
            JOIN
                [dbo].[TrainingCentres] tc ON cust.TrainingCentreId = tc.Id";

            if (userRole != "Super Admin")
            {
                query += @"
            WHERE
                cust.Email = @Email";
            }

            query += @"
            GROUP BY
                c.[Sector],
                tc.Id
            ORDER BY
                Percentage DESC;";

            var command = new SqlCommand(query, connection);

            if (userRole != "Super Admin")
            {
                command.Parameters.AddWithValue("@Email", email);
            }

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    industryList.Add(new IndustriesModel
                    {
                        Industry = reader["Industry"].ToString(),
                        Percentage = Convert.ToDouble(reader["Percentage"])
                    });
                }
            }
        }

        return industryList;
    }



    //public async Task<List<IndustriesModel>> GetIndustryPercentageAsync(string email)
    //{
    //    const string query = @"
    //        SELECT
    //            c.[Sector] AS Industry,
    //            COUNT(*) * 100.0 / SUM(COUNT(*)) OVER (PARTITION BY tc.Id) AS Percentage
    //        FROM
    //            [dbo].[QualificationCustomerCourseCertificates] qccc
    //        JOIN
    //            [dbo].[Courses] c ON qccc.CourseId = c.Id
    //        JOIN
    //            [dbo].[Customers] cust ON qccc.CustomerId = cust.Id
    //        JOIN
    //            [dbo].[TrainingCentres] tc ON cust.TrainingCentreId = tc.Id
    //        WHERE
    //            cust.Email = @Email
    //        GROUP BY
    //            c.[Sector],
    //            tc.Id
    //        ORDER BY
    //            Percentage DESC;
    //    ";

    //    var industryList = new List<IndustriesModel>();

    //    using (var connection = new SqlConnection(_cRMDbContext.Database.GetConnectionString()))
    //    {
    //        await connection.OpenAsync();

    //        using (var command = new SqlCommand(query, connection))
    //        {
    //            command.Parameters.AddWithValue("@Email", email);

    //            using (var reader = await command.ExecuteReaderAsync())
    //            {
    //                while (await reader.ReadAsync())
    //                {
    //                    industryList.Add(new IndustriesModel
    //                    {
    //                        Industry = reader["Industry"].ToString(),
    //                        Percentage = Convert.ToDouble(reader["Percentage"])
    //                    });
    //                }
    //            }
    //        }
    //    }

    //    return industryList;
    //}

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