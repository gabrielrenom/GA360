using GA360.DAL.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Entities
{
    public class ApplicationPermission: Audit, IModel
    {
        public int Id { get; set; }
        public int? TrainingCentreId { get; set; }
        public int? QualificationId { get; set; }
        public int? CourseId { get; set; }
        public int? CertificateId { get; set; }
        public int RoleId {  get; set; }
        public virtual Role Role {  get; set; }
    }
}
