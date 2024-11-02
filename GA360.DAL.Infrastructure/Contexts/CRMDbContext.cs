using CRM.Entities;
using CRM.Entities.Entities;
using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Infrastructure.Contexts
{
    public class CRMDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Client> Clients { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ClientCustomer> ClientContacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CustomerSkills> CustomerSkills { get; set; }
        public DbSet<TrainingCentre> TrainingCentres { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentCertificate> DocumentCertificates { get; set; }
        public DbSet<DocumentTrainingCentre> DocumentTrainingCentres { get; set; }
        public DbSet<EthnicOrigin> EthnicOrigins { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<QualificationCustomerCourseCertificate> QualificationCustomerCourseCertificates { get; set; }
        public DbSet<DocumentCustomer> DocumentCustomer { get; set; }
        public DbSet<QualificationStatus> QualificationStatuses { get; set; }
        public CRMDbContext(DbContextOptions<CRMDbContext> options, IConfiguration configuration)
       : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
                var connectionString = _configuration.GetConnectionString("CRM");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
            .HasOne(c => c.ParentClient)
            .WithMany(c => c.ChildClients)
            .HasForeignKey(c => c.ParentClientId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrainingCentre>()
            .HasOne(tc => tc.Address)
            .WithMany()
            .HasForeignKey(tc => tc.AddressId);

            base.OnModelCreating(modelBuilder);
        }

        public void SeedData()
        {
            CRMDbContextSeed.Initialize(this);
        }
    }
}
