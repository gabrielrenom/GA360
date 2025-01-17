using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Models;

public class CourseTrainingModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; }
    public DateTime ExpectedDate { get; set; }
    public int Duration { get; set; }
    public DateTime CertificateDate { get; set; }
    public string CertificateNumber { get; set; }
    public int Status { get; set; }
    public string Sector { get; set; }
    public int Learners { get; set; }
}
