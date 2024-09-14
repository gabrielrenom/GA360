using GA360.DAL.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Interfaces
{
    public interface IEthnicityService
    {
        Task<List<EthnicOrigin>> GetAll();
    }
}
