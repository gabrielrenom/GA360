using GA360.DAL.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace GA360.DAL.Entities.Entities;

public class Contact : Audit, ITenant
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Phone { get; set; }
    public string Mail { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public Guid? TenantId { get; set; }
    public string FirstName { get; set; }

}
