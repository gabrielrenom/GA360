using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class QualificationTrainingCentre : Audit, IModel
{
    public int Id { get; set; }
    public int TrainingCentreId { get; set; }
    public int QualificationId { get; set; }
    public Qualification Qualification { get; set; }
    public TrainingCentre TrainingCentre { get; set; }
}
