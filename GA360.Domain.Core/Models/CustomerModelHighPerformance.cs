using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Models
{
    public class CustomerModelHighPerformance
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Location { get; set; }
        public int Status { get; set; }
        public int Avatar { get; set; }
        public int CountryId { get; set; }
        public string DOB { get; set; }
        public string DateOfBirth { get; set; }
        public int TrainingCentreId { get; set; }
        public string TrainingCentre { get; set; }
        public string AvatarImage { get; set; }
    }
}
