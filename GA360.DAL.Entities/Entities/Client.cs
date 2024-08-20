using GA360.DAL.Entities.BaseEntities;
using GA360.DAL.Entities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GA360.DAL.Entities.Enums.StatusEnum;

namespace CRM.Entities
{
    public class Client : Audit, ITenant
    {
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        [Required]
        public ClientType Type { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public ClientStatus Status { get; set; }
        public Guid? TenantId { get; set; }
        public long? RegionId { get; set; }
        [ForeignKey(nameof(RegionId))]
        public Region Region { get; set; }
    }
}
