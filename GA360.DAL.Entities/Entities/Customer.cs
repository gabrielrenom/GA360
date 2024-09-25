using GA360.DAL.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;
using static GA360.DAL.Entities.Enums.StatusEnum;

namespace GA360.DAL.Entities.Entities;

public class Customer : Audit, IModel
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Contact { get; set; }
    public string FatherName { get; set; }
    public string About { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string? Description { get; set; }
    public string Location { get; set; }
    public Status Status { get; set; }
    public int CountryId { get; set; }
    public virtual Country Country { get; set; }
    public Guid? TenantId { get; set; }
    public int Id { get; set; }
    public int AddressId { get; set; }
    public string DOB {  get; set; }
    public string NI { get; set; }
    public string Disability { get; set; }
    public string EmploymentStatus { get; set; }
    public string Employer { get; set; }
    public string ePortfolio { get; set; }
    public int EthnicOriginId {  get; set; }
    public string? AvatarImage { get; set; }
    public int? TrainingCentreId { get; set; }
    public virtual Address Address { get; set; }
    public virtual EthnicOrigin EthnicOrigin { get; set; }
    public virtual List<CustomerSkills> CustomerSkills { get; set; }
    public virtual List<QualificationCustomerCourseCertificate>? QualificationCustomerCourseCertificates { get; set; }
    public virtual TrainingCentre TrainingCentre { get; set; }

}
