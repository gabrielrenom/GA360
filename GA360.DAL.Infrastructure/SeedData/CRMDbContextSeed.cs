using CRM.Entities;
using CRM.Entities.Entities;
using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GA360.DAL.Infrastructure.SeedData
{
    public static class CRMDbContextSeed
    {
        public static void Initialize(CRMDbContext context)
        {
            // Check if the Clients table has any data
            if (!context.Clients.Any())
            {
                context.Clients.AddRange(
                    new Client { Id = 1, Name = "Client A", ParentClientId = null },
                    new Client { Id = 2, Name = "Client B", ParentClientId = 1 }
                );
            }

            // Check if the Countries table has any data
            if (!context.Countries.Any())
            {
                context.Countries.AddRange(
                    new Country { Id = 1, Name = "Country A" },
                    new Country { Id = 2, Name = "Country B" }
                );
            }

            context.SaveChanges();
        }
    }
}
