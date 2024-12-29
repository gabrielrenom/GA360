using CRM.Entities;
using CRM.Entities.Entities;
using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Migrations;
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
        public DbSet<CourseTrainingCentre> CourseTrainingCentre { get; set; }
        public DbSet<QualificationTrainingCentre> QualificationTrainingCentre { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ApplicationPermission> ApplicationPermissions { get; set; }
        public DbSet<EntityPermission> EntityPermissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<TrainingCentrePermission> TrainingCentrePermissions { get; set; }
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

//            --Indexes for Customer Table
//            CREATE INDEX IDX_Customer_TrainingCentreId ON Customers(TrainingCentreId);

//            --Indexes for QualificationCustomerCourseCertificates Table
//            CREATE INDEX IDX_QCCC_CustomerId ON QualificationCustomerCourseCertificates(CustomerId);
//            CREATE INDEX IDX_QCCC_CourseId ON QualificationCustomerCourseCertificates(CourseId);
//            CREATE INDEX IDX_QCCC_QualificationId ON QualificationCustomerCourseCertificates(QualificationId);
//            CREATE INDEX IDX_QCCC_CertificateId ON QualificationCustomerCourseCertificates(CertificateId);
//            CREATE INDEX IDX_QCCC_QualificationStatusId ON QualificationCustomerCourseCertificates(QualificationStatusId);

//            --Indexes for TrainingCentres Table
//            CREATE INDEX IDX_TrainingCentres_Id ON TrainingCentres(Id);

//            --Indexes for Courses Table
//            CREATE INDEX IDX_Courses_Id ON Courses(Id);

//            --Indexes for Qualifications Table
//            CREATE INDEX IDX_Qualifications_Id ON Qualifications(Id);

//            --Indexes for Certificates Table
//            CREATE INDEX IDX_Certificates_Id ON Certificates(Id);

//            --Indexes for QualificationStatuses Table
//            CREATE INDEX IDX_QualificationStatuses_Id ON QualificationStatuses(Id);
            //// Indexes for Customer Table
            //modelBuilder.Entity<Customer>()
            //    .HasIndex(c => c.TrainingCentreId)
            //    .HasDatabaseName("IDX_Customer_TrainingCentreId");

            //// Indexes for QualificationCustomerCourseCertificates Table
            //modelBuilder.Entity<QualificationCustomerCourseCertificate>()
            //    .HasIndex(qccc => qccc.CustomerId)
            //    .HasDatabaseName("IDX_QCCC_CustomerId");

            //modelBuilder.Entity<QualificationCustomerCourseCertificate>()
            //    .HasIndex(qccc => qccc.CourseId)
            //    .HasDatabaseName("IDX_QCCC_CourseId");

            //modelBuilder.Entity<QualificationCustomerCourseCertificate>()
            //    .HasIndex(qccc => qccc.QualificationId)
            //    .HasDatabaseName("IDX_QCCC_QualificationId");

            //modelBuilder.Entity<QualificationCustomerCourseCertificate>()
            //    .HasIndex(qccc => qccc.CertificateId)
            //    .HasDatabaseName("IDX_QCCC_CertificateId");

            //modelBuilder.Entity<QualificationCustomerCourseCertificate>()
            //    .HasIndex(qccc => qccc.QualificationStatusId)
            //    .HasDatabaseName("IDX_QCCC_QualificationStatusId");

            //// Indexes for TrainingCentres Table
            //modelBuilder.Entity<TrainingCentre>()
            //    .HasIndex(tc => tc.Id)
            //    .HasDatabaseName("IDX_TrainingCentres_Id");

            //// Indexes for Courses Table
            //modelBuilder.Entity<Course>()
            //    .HasIndex(co => co.Id)
            //    .HasDatabaseName("IDX_Courses_Id");

            //// Indexes for Qualifications Table
            //modelBuilder.Entity<Qualification>()
            //    .HasIndex(q => q.Id)
            //    .HasDatabaseName("IDX_Qualifications_Id");

            //// Indexes for Certificates Table
            //modelBuilder.Entity<Certificate>()
            //    .HasIndex(qc => qc.Id)
            //    .HasDatabaseName("IDX_Certificates_Id");

            //// Indexes for QualificationStatuses Table
            //modelBuilder.Entity<QualificationStatus>()
            //    .HasIndex(qs => qs.Id)
            //    .HasDatabaseName("IDX_QualificationStatuses_Id");
            base.OnModelCreating(modelBuilder);
        }

        public void SeedData()
        {
            CRMDbContextSeed.Initialize(this);
        }
    }
}
