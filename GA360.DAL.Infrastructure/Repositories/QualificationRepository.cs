using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Infrastructure.Repositories;

public class QualificationRepository : Repository<Qualification>, IQualificationRepository
{

    public QualificationRepository(CRMDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<QualificationStatus>> GetQualificationStatusAsync()
    {
        return await GetDbContext()?.QualificationStatuses.ToListAsync();
    }

    public async Task<List<Qualification>> GetQualificationsForCustomerAsync(int customerId)
    {
        var dbContext = GetDbContext();

        // Query to get qualifications for the customer
        var qualifications = await dbContext.Qualifications
            .Join(dbContext.EntityPermissions, q => q.Id, ep => ep.EntityId, (q, ep) => new { q, ep })
            .Join(dbContext.RolePermissions, qep => qep.ep.PermissionId, rp => rp.PermissionId, (qep, rp) => new { qep.q, qep.ep, rp })
            .Join(dbContext.UserRoles, qepr => qepr.rp.RoleId, ur => ur.RoleId, (qepr, ur) => new { qepr.q, qepr.ep, qepr.rp, ur })
            .Where(qepru => qepru.ur.CustomerId == customerId && qepru.ep.EntityType == "Qualification")
            .Select(qepru => qepru.q)
            .Distinct()
            .ToListAsync();

        return qualifications;

        //var customerId = 5;

        //var qualifications = from q in dbContext.Qualifications
        //                     join ep in dbContext.EntityPermissions on new { Id = q.Id, EntityType = "Qualification" } equals new { Id = ep.EntityId, ep.EntityType }
        //                     join rp in dbContext.RolePermissions on ep.PermissionId equals rp.PermissionId
        //                     join ur in dbContext.UserRoles on rp.RoleId equals ur.RoleId
        //                     where ur.CustomerId == customerId
        //                       && ep.IsDeleted == false
        //                       && rp.IsDeleted == false
        //                       && ur.IsDeleted == false
        //                     select q;

        //var result = qualifications.ToList();

    }
}