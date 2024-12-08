using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.SeedData;

namespace GA360.DAL.Migrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<CRMDbContext>();
                    context.Database.Migrate(); // Apply any pending migrations
                    context.SeedData(); // Seed the data
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred seeding the DB: {ex.Message}");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var connectionString = configuration.GetConnectionString("CRM");

                    services.AddDbContext<CRMDbContext>(options =>
                        options.UseSqlServer(connectionString));
                });
    }
}