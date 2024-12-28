using GA360.DAL.Entities.BaseEntities;
using GA360.DAL.Entities.Entities;

namespace GA360.Server.ViewModels;

public class QualificationViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime ExpectedDate { get; set; }
    public DateTime CertificateDate { get; set; }
    public int CertificateNumber { get; set; }
    public int Status { get; set; }
    public int TrainingCentreId { get; set; }
    public string TrainingCentre { get; set; }
}


public static class QualificationMapper
{
    public static QualificationViewModel ToViewModel(this Qualification qualification)
    {
        return new QualificationViewModel
        {
            Id = qualification.Id,
            Name = qualification.Name,
            RegistrationDate = qualification.RegistrationDate,
            ExpectedDate = qualification.ExpectedDate,
            CertificateDate = qualification.CertificateDate,
            CertificateNumber = qualification.CertificateNumber,
            Status = qualification.Status
        };
    }
    public static Qualification ToEntity(this QualificationViewModel qualificationViewModel)
    {
        return new Qualification
        {
            Id = qualificationViewModel.Id,
            Name = qualificationViewModel.Name,
            RegistrationDate = qualificationViewModel.RegistrationDate,
            ExpectedDate = qualificationViewModel.ExpectedDate,
            CertificateDate = qualificationViewModel.CertificateDate,
            CertificateNumber = qualificationViewModel.CertificateNumber,
            Status = qualificationViewModel.Status,
            QualificationCustomerCourseCertificates = new List<QualificationCustomerCourseCertificate>() // Initialize with an empty list or map accordingly
        };
    }
}
