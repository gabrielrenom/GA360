using GA360.DAL.Entities.BaseEntities;
using static GA360.DAL.Entities.Enums.StatusEnum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GA360.DAL.Entities.Entities
{
    public class Lead: Audit, ITenant
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string LeadSource { get; set; }
        public LeadStatus Status { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? TenantId { get; set; }
    }
}
