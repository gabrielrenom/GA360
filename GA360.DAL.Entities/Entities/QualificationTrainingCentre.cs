using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class QualificationTrainingCentre : Audit, IModel
{
    public int Id { get; set; }
    public int TrainingCentreId { get; set; }
    public int QualificationId { get; set; }
    // Initial price
    public double? Price { get; set; }
    // Discount
    public double? Discount { get; set; }
    // price+discount
    public double? Sale { get; set; }
    public Qualification Qualification { get; set; }
    public TrainingCentre TrainingCentre { get; set; }
}
