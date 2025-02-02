using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class QualificationCustomerCourseCertificate: Audit, IModel
{
    public int Id { get; set; }
    public int? CourseId { get; set; }
    public int? CustomerId { get; set; }
    public int? QualificationId { get; set; }
    public int? CertificateId { get; set;}
    public int QualificationProgression {  get; set; } = 0;
    public int CourseProgression {  get; set; } = 0;
    public int? QualificationStatusId { get; set; } = 0;
    public string Assesor {  get; set; } = string.Empty;
    // Initial price
    public double? CoursePrice { get; set; }
    // Discount
    public double? CourseDiscount { get; set; }
    // price+discount
    public double? CourseSale { get; set; }
    // Initial price
    public double? QualificationPrice { get; set; }
    // Discount
    public double? QualificationDiscount { get; set; }
    public DateTime? QualificationRegistrationDate { get; set; }
    public DateTime? CourseRegistrationDate { get; set; }
    public DateTime? CourseExpectedDate { get; set; }
    public DateTime? CourseCertificateDate { get; set; }
    public string? CertificateNumber { get; set; }
    // price+discount
    public double? QualificationSale { get; set; }
    public virtual Course Course { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual Qualification Qualification { get; set; }
    public virtual Certificate Certificate { get; set; }
    public virtual QualificationStatus QualificationStatus { get; set; }
}
