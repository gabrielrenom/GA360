using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class Country : Audit, IModel
{
    public int Id { get; set; }
    public Guid? TenantId { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
    public string? Prefix { get; set; }
}
