using GA360.DAL.Entities.BaseEntities;


namespace GA360.DAL.Entities.Entities;

public class Role : Audit, IModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual List<ApplicationPermission> Permissions { get; set; }

    public virtual List<UserRole> UserRoles { get; set; }
    public virtual List<RolePermission> RolePermissions { get; set; }

}
