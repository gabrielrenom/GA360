using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class DocumentCustomer : Audit, IModel
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public int CustomerId { get; set; }

    public virtual Document Document { get; set; }
    public virtual Customer Customer { get; set; }
}