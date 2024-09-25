using GA360.DAL.Entities.Entities;

namespace GA360.Domain.Core.Interfaces
{
    public interface IEthnicityService
    {
        Task<List<EthnicOrigin>> GetAll();
    }
}
