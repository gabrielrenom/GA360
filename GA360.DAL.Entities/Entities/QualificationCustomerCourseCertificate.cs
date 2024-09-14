﻿using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class QualificationCustomerCourseCertificate: Audit, IModel
{
    public int Id { get; set; }
    public int? CourseId { get; set; }
    public int? CustomerId { get; set; }
    public int? QualificationId { get; set; }
    public int? CertificateId { get; set;}
    public virtual Course Course { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual Qualification Qualification { get; set; }
    public virtual Certificate Certificate { get; set; }
}
