

using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class Region : Audit, ITenant
{ 
    public Guid? TenantId { get; set; }
    public string Name { get; set; }
}
