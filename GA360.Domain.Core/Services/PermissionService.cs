using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;


public class PermissionService:IPermissionService
{
    private readonly CRMDbContext _context;

    public PermissionService(CRMDbContext context)
    {
        _context = context;
    }

    public async Task<PermissionModel> GetPermissions(string email)
    {
        var userRoles = await _context
            .UserRoles
            .Include(x => x.Role)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Customer.Email.ToLower() == email.ToLower())
            .ToListAsync();

        if (userRoles == null || !userRoles.Any())
        {
            return null; // Or throw an exception if preferred
        }

        var userRole = userRoles.FirstOrDefault();

        var permissionModel = new PermissionModel
        {
            CustomerId = userRole.CustomerId,
            Role = userRole.Role.Name,
            RoleId = userRole.RoleId,
            PermissionEntities = userRole.Role.Permissions.Select(p => new PermissionEntity
            {
                TrainingCentreId = p.TrainingCentreId ?? 0,
                CourseId = p.CourseId ?? 0,
                QualificationId = p.QualificationId ?? 0,
                CertificateId = p.CertificateId ?? 0
            }).ToList()
        };

        return permissionModel;
    }

    public async Task<List<Course>> FilterPermissions(string email, List<Course> courses)
    {
        var trainingCentre = await _context
            .TrainingCentres
            .Where(x => x.Customers.Any(c => c.Email.ToLower() == email.ToLower()))
            .FirstAsync();

        var permissions = await GetPermissions(email);

        if (permissions == null || permissions.PermissionEntities == null)
        {
            return new List<Course>(); // No permissions, return empty list
        }

        var allowedCourses = courses.Where(course =>
            permissions.PermissionEntities.Any(pe => pe.CourseId == course.Id && pe.TrainingCentreId == trainingCentre.Id)).ToList();

        return allowedCourses;
    }

    public async Task<List<Certificate>> FilterPermissions(string email, List<Certificate> certificates)
    {
        var trainingCentre = await _context
            .TrainingCentres
            .Where(x => x.Customers.Any(c => c.Email.ToLower() == email.ToLower()))
            .FirstAsync();

        var permissions = await GetPermissions(email);

        if (permissions == null || permissions.PermissionEntities == null)
        {
            return new List<Certificate>(); // No permissions, return empty list
        }

        var allowedCourses = certificates.Where(certificate =>
            permissions.PermissionEntities.Any(pe => pe.CertificateId == certificate.Id && pe.TrainingCentreId == trainingCentre.Id)).ToList();

        return allowedCourses;
    }

    public async Task<List<Qualification>> FilterPermissions(string email, List<Qualification> qualifications)
    {
        var trainingCentre = await _context
            .TrainingCentres
            .Where(x => x.Customers.Any(c => c.Email.ToLower() == email.ToLower()))
            .FirstAsync();

        var permissions = await GetPermissions(email);

        if (permissions == null || permissions.PermissionEntities == null)
        {
            return new List<Qualification>(); // No permissions, return empty list
        }

        var allowedQualifications = qualifications.Where(qualification =>
            permissions.PermissionEntities.Any(pe => pe.QualificationId == qualification.Id && pe.TrainingCentreId == trainingCentre.Id)).ToList();

        return allowedQualifications;
    }

}
