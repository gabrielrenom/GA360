using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class UserRole : Audit, IModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int RoleId { get; set; }
    public Customer Customer { get; set; }
    public virtual Role Role { get; set; }

}
