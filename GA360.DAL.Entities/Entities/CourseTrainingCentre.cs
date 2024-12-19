using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Entities
{
    public class CourseTrainingCentre
    {
        public int Id { get; set; }
        public int TrainingCentreId { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public TrainingCentre TrainingCentre { get; set; }
    }
}
