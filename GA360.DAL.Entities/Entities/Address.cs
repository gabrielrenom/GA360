using GA360.DAL.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Entities;

public class Address : Audit, IModel
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string Postcode { get; set; }
    public string City { get; set; }
}
