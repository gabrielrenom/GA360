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
    public DealStatus OrderStatus { get; set; }
    public int CountryId { get; set; }
    public virtual Country Country { get; set; }
    public Guid? TenantId { get; set; }
    public int Id { get; set; }
    public List<CustomerSkills> CustomerSkills { get; set; }
}
