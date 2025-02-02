using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities
{


    public class AuditTrail : Audit, IModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public string Level { get; set; }
        public string From { get; set; }
    }
}
