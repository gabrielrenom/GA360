using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Models;


public class CourseDetailsModel
{
    public int Id { get; set; }
    public int Status { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Progression { get; set; }
    public string Assesor { get; set; }
    public int Duration { get; set; }
    public string Date { get; set; }
    public string Card { get; set; } = string.Empty;
    public string Certification { get; set; } = string.Empty;
    public int? TrainingCentreId { get; set; }
    public string? TrainingCentre { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime ExpectedDate { get; set; }
    public DateTime CertificateDate { get; set; }
    public string CertificateNumber { get; set; }
    public string Sector { get; set; }
    public int? Learners { get; set; }
    public double? Price { get; set; }
}

