using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class Qualification: Audit, IModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime ExpectedDate { get; set; }
    public DateTime CertificateDate { get; set; }
    public int CertificateNumber { get; set; }
    public int Status { get; set; }

    public virtual List<QualificationCustomerCourseCertificate> QualificationCustomerCourseCertificates { get; set; }

}
