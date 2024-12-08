using GA360.DAL.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Entities;

public class DocumentCertificate: Audit, IModel
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public int CertificateId { get; set; }

    public virtual Certificate Certificate { get; set; }
    public virtual Document Document { get; set; }
}