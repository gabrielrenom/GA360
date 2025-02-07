using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class QualificationStatus : Audit, IModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public virtual List<QualificationCustomerCourseCertificate>? QualificationCustomerCourseCertificates { get; set; }
}