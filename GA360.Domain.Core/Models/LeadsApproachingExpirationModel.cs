using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Models;

public class LeadsApproachingExpirationModel
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string DateAdded { get; set; }
    public string ExpiryDate { get; set; }
    public string Status { get; set; }
}
