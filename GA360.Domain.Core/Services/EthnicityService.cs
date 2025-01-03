﻿using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;

namespace GA360.Domain.Core.Services;

public class EthnicityService : IEthnicityService
{
    private IEthnicityRepository _ethnicityRepository;

    public EthnicityService(IEthnicityRepository ethnicityRepository)
    {
        _ethnicityRepository = ethnicityRepository;
    }

    public async Task<List<EthnicOrigin>> GetAll()
    {
        return await _ethnicityRepository.GetAll();
    }
}
