using GA360.DAL.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Models;

public class PermissionModel
{
    public int CustomerId { get; set; }
    public string Role { get; set; }
    public string CustomerEmail { get; set; }
    public int RoleId { get; set; }
    public List<PermissionEntity> PermissionEntities { get; set; }
}

public class PermissionEntity
{
    public int TrainingCentreId { get; set; }
    public int CourseId { get; set; }
    public int QualificationId { get; set; }
    public int CertificateId { get; set; }
}
