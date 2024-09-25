using GA360.DAL.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Entities
{
    public class EthnicOrigin : Audit, IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Customer> Customers { get; set; }
    }
}
