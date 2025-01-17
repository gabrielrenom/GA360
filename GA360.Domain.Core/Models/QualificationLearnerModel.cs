namespace GA360.Domain.Core.Models;

public class QualificationLearnerModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public DateTime? CertificateDate { get; set; }
    public int? CertificateNumber { get; set; }
    public int Status { get; set; }
    public string? AwardingBody { get; set; }
    public string? InternalReference { get; set; }
    public string? QAN { get; set; }
    public string? Sector { get; set; }
    public int? Progression { get; set; }
    public string? Assessor { get; set; }
    public double? Price { get; set; }
}
