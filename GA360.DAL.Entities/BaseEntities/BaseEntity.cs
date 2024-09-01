using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.BaseEntities
{
    public abstract class BaseEntity: ITenant
    {
        public string CreatedBy { get; set; } = "System";
        public string ModyfiedBy { get; set; } = "System";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public Guid? TenantId { get; set; }
    }
}
