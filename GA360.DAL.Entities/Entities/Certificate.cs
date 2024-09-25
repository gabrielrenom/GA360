using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class Certificate:Audit, IModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Charge { get; set; }
    public int AddressId { get; set; }

    public virtual List<QualificationCustomerCourseCertificate> QualificationCustomerCourseCertificates { get; set; }
    public virtual List<DocumentCertificate> DocumentCertificates { get; set; }
}
