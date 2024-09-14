using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Services
{
    public class EthnicityService : IEthnicityService
    {
        private IEthnicityRepository _ethnicityRepository;
        public async Task<List<EthnicOrigin>> GetAll()
        {
            return await _ethnicityRepository.GetAll();
        }
    }
}
