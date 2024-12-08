using GA360.DAL.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Entities
{
    public class EntityPermission : Audit, IModel
    {
        public int Id { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }

}
