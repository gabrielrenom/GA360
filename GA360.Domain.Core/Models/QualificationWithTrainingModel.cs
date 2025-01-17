using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Models;

public class QualificationWithTrainingModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public DateTime? CertificateDate { get; set; }
    public int? CertificateNumber { get; set; }
    public int Status { get; set; }
    public int? TrainingCentreId { get; set; }
    public string? TrainingCentre { get; set; }  // Assuming training centre's name as string
    public string? InternalReference { get; set; }
    public string? QAN { get; set; }
    public int? Learners { get; set; }
    public string? AwardingBody { get; set; }
    public double? Price { get; set; }
    public string? Sector { get; set; }
}
