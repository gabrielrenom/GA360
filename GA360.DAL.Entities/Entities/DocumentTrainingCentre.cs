using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class DocumentTrainingCentre : Audit, IModel
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public int TrainingCentreId { get; set; }

    public virtual Document Document { get; set; }
    public virtual TrainingCentre TrainingCentre { get; set; }
}
