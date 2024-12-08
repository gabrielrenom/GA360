using GA360.DAL.Entities.BaseEntities;


namespace GA360.DAL.Entities.Entities;

public class TrainingCentrePermission : Audit, IModel
{
    public int Id { get; set; }
    public int TrainingCentreId { get; set; }
    public int PermissionId { get; set; }
    public virtual TrainingCentre TrainingCentre { get; set; }
    public virtual Permission Permission { get; set; }
}
