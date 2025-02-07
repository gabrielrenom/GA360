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

    public async Task<List<IndustriesModel>> GetIndustryPercentageAsync(string email)
    {
        var industryList = new List<IndustriesModel>();

        using (var connection = new SqlConnection(_cRMDbContext.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT 
                Sector,
                COUNT(*) AS QualificationCount,
                (COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Qualifications WHERE Sector IS NOT NULL)) AS Percentage
            FROM Qualifications
            WHERE Sector IS NOT NULL
            GROUP BY Sector";

            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        industryList.Add(new IndustriesModel
                        {
                            Industry = reader["Sector"].ToString(),
                            Percentage = Convert.ToDouble(reader["Percentage"])
                        });
                    }
                }
            }
        }

        return industryList;
    }


    public async Task<List<IndustriesModel>> GetIndustryPercentageByTrainingCentreIdAsync(int trainingCentreId)
    {
        var industryList = new List<IndustriesModel>();

        using (var connection = new SqlConnection(_cRMDbContext.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT 
                q.Sector,
                COUNT(*) AS QualificationCount,
                (COUNT(*) * 100.0 / (
                    SELECT COUNT(*) 
                    FROM QualificationTrainingCentre qtc
                    INNER JOIN Qualifications q ON qtc.QualificationId = q.Id
                    WHERE qtc.TrainingCentreId = @TrainingCentreId AND q.Sector IS NOT NULL
                )) AS Percentage
            FROM QualificationTrainingCentre qtc
            INNER JOIN Qualifications q ON qtc.QualificationId = q.Id
            WHERE qtc.TrainingCentreId = @TrainingCentreId AND q.Sector IS NOT NULL
            GROUP BY q.Sector";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        industryList.Add(new IndustriesModel
                        {
                            Industry = reader["Sector"].ToString(),
                            Percentage = Convert.ToDouble(reader["Percentage"])
                        });
                    }
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

    public async Task<List<DashboardModel>> GetAllStats(int? trainingCentreId = null)
    {
        return new List<DashboardModel>
        {
            await GetCompletedLearners(trainingCentreId),
            await GetActiveLearners(trainingCentreId),
            await GetRegistrations(trainingCentreId),
            await GetNewLearners(trainingCentreId),
            await GetTheNewLeadsFromThisMonthWithThePercentageIncreaseFromLastMonthAndTotalThisYear(trainingCentreId)
        };
    }

    public async Task<DashboardModel> GetTheNewLeadsFromThisMonthWithThePercentageIncreaseFromLastMonthAndTotalThisYear(int? trainingCentreId)
    {
        var dashboardModel = new DashboardModel
        {
            StatisticType = StatisticType.NEW_LEADS
        };

        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            // Define the WHERE clause based on the trainingCentreId
            string whereClause = trainingCentreId.HasValue
                ? "AND c.TrainingCentreId = @TrainingCentreId"
                : "";

            // Get the total leads for this month (distinct customers without QualificationId)
            string queryThisMonth = $@"
            SELECT COUNT(DISTINCT c.Id) 
            FROM Customers c
            LEFT JOIN QualificationCustomerCourseCertificates qccc ON c.Id = qccc.CustomerId
            WHERE qccc.QualificationId IS NULL 
            AND MONTH(c.CreatedAt) = MONTH(GETDATE()) 
            AND YEAR(c.CreatedAt) = YEAR(GETDATE()) 
            {whereClause}";

            using (SqlCommand command = new SqlCommand(queryThisMonth, connection))
            {
                if (trainingCentreId.HasValue)
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId.Value);
                }
                dashboardModel.Total = (int)await command.ExecuteScalarAsync();
            }

            // Get the total leads for last month (distinct customers without QualificationId)
            string queryLastMonth = $@"
            SELECT COUNT(DISTINCT c.Id) 
            FROM Customers c
            LEFT JOIN QualificationCustomerCourseCertificates qccc ON c.Id = qccc.CustomerId
            WHERE qccc.QualificationId IS NULL 
            AND MONTH(c.CreatedAt) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
            AND YEAR(c.CreatedAt) = YEAR(GETDATE()) 
            {whereClause}";

            decimal totalLastMonth = 0;
            using (SqlCommand command = new SqlCommand(queryLastMonth, connection))
            {
                if (trainingCentreId.HasValue)
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId.Value);
                }
                totalLastMonth = (int)await command.ExecuteScalarAsync();
            }

            // Get the total leads for this year (distinct customers without QualificationId)
            string queryTotalThisYear = $@"
            SELECT COUNT(DISTINCT c.Id) 
            FROM Customers c
            LEFT JOIN QualificationCustomerCourseCertificates qccc ON c.Id = qccc.CustomerId
            WHERE qccc.QualificationId IS NULL 
            AND YEAR(c.CreatedAt) = YEAR(GETDATE()) 
            {whereClause}";

            using (SqlCommand command = new SqlCommand(queryTotalThisYear, connection))
            {
                if (trainingCentreId.HasValue)
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId.Value);
                }
                dashboardModel.TotalYear = (int)await command.ExecuteScalarAsync();
            }

            // Calculate the percentage increase from last month
            if (totalLastMonth > 0)
            {
                dashboardModel.Percentage = ((dashboardModel.Total - totalLastMonth) / totalLastMonth) * 100;
            }
            else
            {
                dashboardModel.Percentage = 100; // If there were no leads last month, treat it as 100% increase
            }
        }

        return dashboardModel;
    }


    public async Task<DashboardModel> GetActiveLearners(int? trainingCentreId = null)
    {
        var dashboardModel = new DashboardModel
        {
            StatisticType = StatisticType.ACTIVE_LEARNERS
        };

        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetActiveLearners]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add the parameter for trainingCentreId
                if (trainingCentreId.HasValue)
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", DBNull.Value);
                }

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            dashboardModel.Total = Convert.ToInt32(reader["TotalCandidates"]);
                            dashboardModel.TotalYear = Convert.ToInt32(reader["CandidatesLast365Days"]);
                            dashboardModel.Percentage = Convert.ToDecimal(reader["PercentageChangeLastMonth"]);
                        }
                    }
                    else
                    {
                        dashboardModel.Percentage = 0;
                            dashboardModel.Total = 0;
                        dashboardModel.TotalYear =0 ;
                    }
                }
            }
        }

        return dashboardModel;
    }
    public async Task<DashboardModel> GetTrainingCentreStats(int? trainingCentreId = null)
    {
        var dashboardModel = new DashboardModel();
        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetCompletedCustomerStats]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add the parameter for trainingCentreId
                if (trainingCentreId.HasValue)
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", DBNull.Value);
                }

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
                            StatisticType = StatisticType.TRAINING_CENTRES_STATS,
                            Total = 0,
                            TotalYear = 0
                        };
                    }
                }
            }
        }

        return dashboardModel;
    }


    public async Task<DashboardModel> GetCompletedLearners(int? trainingCentreId = null)
    {
        var dashboardModel = new DashboardModel();
        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetCompletedCustomerStats]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add the parameter for trainingCentreId
                if (trainingCentreId.HasValue)
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", DBNull.Value);
                }

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
                            StatisticType = StatisticType.COMPLETED_LEARNERS,
                            Total = 0,
                            TotalYear = 0
                        };
                    }
                }
            }
        }

        return dashboardModel;
    }

    public async Task<DashboardModel> GetRegistrations(int? trainingCentreId = null)
    {
        var dashboardModel = new DashboardModel();
        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetCustomerRegistrationStats]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add the parameter for trainingCentreId
                if (trainingCentreId.HasValue)
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", DBNull.Value);
                }

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
                                StatisticType = StatisticType.CANDIDATE_REGISTRATIONS,
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
                            StatisticType = StatisticType.CANDIDATE_REGISTRATIONS,
                            Total = 0,
                            TotalYear = 0
                        };
                    }
                }
            }
        }

        return dashboardModel;
    }


    public async Task<DashboardModel> GetNewLearners(int? trainingCentreId = null)
    {
        var dashboardModel = new DashboardModel();
        string connectionString = _cRMDbContext.Database.GetDbConnection().ConnectionString;
        string storedProcedureName = "[dbo].[GetCandidateStats]";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add the parameter for trainingCentreId
                if (trainingCentreId.HasValue)
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", trainingCentreId.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@TrainingCentreId", DBNull.Value);
                }

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