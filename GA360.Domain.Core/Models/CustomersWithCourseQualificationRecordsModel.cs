namespace GA360.Domain.Core.Models;

public class CustomersWithCourseQualificationRecordsModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Email { get; set; }
    public int? CourseId { get; set; }
    public string CourseName { get; set; }
    public int? QualificationId { get; set; }
    public string QualificationName { get; set; }
    public int? CertificateId { get; set; }
    public string CertificateName { get; set; }
    public int? TrainingCentreId { get; set; }
    public string TrainingCentre { get; set; }
    public int Progression { get; set; }
    public int? QualificationStatusId { get; set; }
    public string QualificationStatus { get; set; }
    public double? QualificationPrice { get; set; }
    public double? CoursePrice { get; set; }
}