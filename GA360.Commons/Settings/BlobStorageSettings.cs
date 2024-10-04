using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Commons.Settings
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; }
        public string DocumentsContainerName { get; set; }
        public string CertificatesContainerName { get; set; }
        public string SharedAccessSignature { get; set; }
    }
}
