using GA360.DAL.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Interfaces
{
  
    public interface IAuditTrailService
    {
        Task<List<AuditTrail>> GetAuditTrails();
        Task InsertAudit(string area, string type, string message, string? source);
        public static class AuditTrailType
        {
            public const string Information = "Information";
            public const string Warning = "Warning";
            public const string Error = "Error";
        }

        public static class AuditTrailArea
        {
            public const string Learners = "Learners";
            public const string Leads = "Leads";
            public const string Courses = "Courses";
            public const string Country = "Country";
            public const string Certificates = "Certificate";
            public const string Ethnicity = "Ethnicity";
            public const string Qualifications = "Qualifications";
            public const string Skills = "Skills";
            public const string TrainingCentre = "Skills";
            public const string Dashboard = "Dashboard";
        }
    }
}
