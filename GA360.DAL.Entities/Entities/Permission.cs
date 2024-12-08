using GA360.DAL.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Entities;

public class Permission : Audit, IModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual List<RolePermission> RolePermissions { get; set; }
    public virtual List<EntityPermission> ? EntityPermissions { get; set; }   
}
