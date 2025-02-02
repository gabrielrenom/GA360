using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Services;

public class AuditTrailService : IAuditTrailService
{
    private readonly CRMDbContext _dbContext;

    public AuditTrailService(CRMDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<AuditTrail>> GetAuditTrails()
    {
        return await _dbContext.AuditTrails
            .OrderByDescending(at => at.CreatedAt)
            .Take(100)
            .ToListAsync();
    }


    public async Task InsertAudit(string area, string type, string message, string? source)
    {
        var auditTrail = new AuditTrail
        {
            Area = area,
            Description = message,
            Level = type,
            From = source,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.AuditTrails.Add(auditTrail);
        await _dbContext.SaveChangesAsync();
    }
}
