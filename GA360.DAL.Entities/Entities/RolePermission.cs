using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class RolePermission : Audit, IModel
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public virtual Permission Permission { get; set; }
    public virtual Role Role { get; set; }
}
