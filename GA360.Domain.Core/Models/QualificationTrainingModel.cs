using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Models
{
    public class QualificationTrainingModel
    {
        public int TrainingCentreId { get; set; }
        public int QualificationId { get; set; }
        public int QAN { get; set; }
        public string InternalReference { get; set; }
        public string QualificationName { get; set; }
        public string AwardingBody { get; set; }
        public int Learners { get; set; }
        public int Assessors { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Status { get; set; }
    }
}
