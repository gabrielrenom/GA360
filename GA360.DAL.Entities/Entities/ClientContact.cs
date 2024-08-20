using GA360.DAL.Entities.BaseEntities;
using GA360.DAL.Entities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Entities.Entities;

public class ClientContact : Audit, ITenant
{
    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }
    [Required]
    public long ClientId { get; set; }
    [ForeignKey(nameof(ContactId))]
    public Contact Contact { get; set; }
    [Required]
    public long ContactId { get; set; }
    public Guid? TenantId { get; set; }
}
