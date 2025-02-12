using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class TrainingCentre : Audit, IModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int AddressId { get; set; }
    public string? Logo { get; set; }
    public virtual Address Address { get; set; }
    public virtual List<Customer> Customers { get; set; }
    public virtual List<TrainingCentrePermission> TrainingCentrePermission { get; set; }
    public virtual List<QualificationTrainingCentre> QualificationTrainingCentres { get; set; }
    public virtual List<CourseTrainingCentre> CourseTrainingCentres { get; set; }
}