using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


public class PermissionService:IPermissionService
{
    private readonly CRMDbContext _context;

    public PermissionService(CRMDbContext context)
    {
        _context = context;
    }

    //public async Task<PermissionModel> GetPermissions(string email)
    //{
    //    var permissionModel = new PermissionModel();

    //    using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
    //    {
    //        await connection.OpenAsync();

    //        // Fetch the user role and permissions
    //        var query = @"
    //        SELECT ur.CustomerId, ur.RoleId, r.Name AS RoleName, 
    //               p.TrainingCentreId, p.CourseId, p.QualificationId, p.CertificateId
    //        FROM [dbo].[UserRoles] ur
    //        JOIN [dbo].[Roles] r ON ur.RoleId = r.Id
    //        LEFT JOIN [dbo].[RolePermissions] rp ON r.Id = rp.RoleId
    //        LEFT JOIN [dbo].[Permissions] p ON rp.PermissionId = p.Id
    //        JOIN [dbo].[Customers] cust ON ur.CustomerId = cust.Id
    //        WHERE LOWER(cust.Email) = @Email";

    //        using (var command = new SqlCommand(query, connection))
    //        {
    //            command.Parameters.AddWithValue("@Email", email.ToLower());

    //            using (var reader = await command.ExecuteReaderAsync())
    //            {
    //                if (await reader.ReadAsync())
    //                {
    //                    var permissions = new List<PermissionEntity>();
    //                    do
    //                    {
    //                        permissions.Add(new PermissionEntity
    //                        {
    //                            TrainingCentreId = reader.IsDBNull(reader.GetOrdinal("TrainingCentreId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TrainingCentreId")),
    //                            CourseId = reader.IsDBNull(reader.GetOrdinal("CourseId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CourseId")),
    //                            QualificationId = reader.IsDBNull(reader.GetOrdinal("QualificationId")) ? 0 : reader.GetInt32(reader.GetOrdinal("QualificationId")),
    //                            CertificateId = reader.IsDBNull(reader.GetOrdinal("CertificateId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CertificateId"))
    //                        });
    //                    } while (await reader.ReadAsync());

    //                    permissionModel.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
    //                    permissionModel.RoleId = reader.GetInt32(reader.GetOrdinal("RoleId"));
    //                    permissionModel.Role = reader.GetString(reader.GetOrdinal("RoleName"));
    //                    permissionModel.PermissionEntities = permissions;
    //                }
    //                else
    //                {
    //                    return null;
    //                }
    //            }
    //        }
    //    }

    //    return permissionModel;
    //}


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
            return null;
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

    public async Task<PermissionModel> UpsertPermissions(PermissionModel permissionModel)
    {
        var userRoles = await _context
            .UserRoles
            .Include(x => x.Role)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Customer.Email.ToLower() == permissionModel.CustomerEmail.ToLower())
            .Include(x => x.Customer)
            .ToListAsync();

        if (userRoles == null || !userRoles.Any())
        {
            throw new Exception("User roles not found for the given email.");
        }

        var roleId = userRoles.First().RoleId;
        var trainingId = userRoles.First().Customer.TrainingCentreId;
        var customerId = userRoles.First().Customer.Id;

        var newPermissions = new List<PermissionEntity>();

        foreach (var permissionEntity in permissionModel.PermissionEntities)
        {
            var existingPermission = await _context.ApplicationPermissions
                .Where(ap => ap.RoleId == roleId && ap.Role.UserRoles.Any(x => x.CustomerId == customerId))
                .Where(ap => (ap.TrainingCentreId == permissionEntity.TrainingCentreId && permissionEntity.TrainingCentreId != 0)
                    || (ap.CourseId == permissionEntity.CourseId && permissionEntity.CourseId != 0)
                    || (ap.QualificationId == permissionEntity.QualificationId && permissionEntity.QualificationId != 0)
                    || (ap.CertificateId == permissionEntity.CertificateId && permissionEntity.CertificateId != 0))
                .FirstOrDefaultAsync();

            if (existingPermission != null)
            {
                // Update the existing permission
                existingPermission.TrainingCentreId = permissionEntity.TrainingCentreId != 0 ? permissionEntity.TrainingCentreId : existingPermission.TrainingCentreId;
                existingPermission.CourseId = permissionEntity.CourseId != 0 ? permissionEntity.CourseId : existingPermission.CourseId;
                existingPermission.QualificationId = permissionEntity.QualificationId != 0 ? permissionEntity.QualificationId : existingPermission.QualificationId;
                existingPermission.CertificateId = permissionEntity.CertificateId != 0 ? permissionEntity.CertificateId : existingPermission.CertificateId;

                _context.ApplicationPermissions.Update(existingPermission);
            }
            else
            {
                // Add a new permission
                var newPermission = new ApplicationPermission
                {
                    RoleId = roleId,
                    TrainingCentreId = trainingId,
                    CourseId = permissionEntity.CourseId != 0 ? permissionEntity.CourseId : (int?)null,
                    QualificationId = permissionEntity.QualificationId != 0 ? permissionEntity.QualificationId : (int?)null,
                    CertificateId = permissionEntity.CertificateId != 0 ? permissionEntity.CertificateId : (int?)null
                };

                await _context.ApplicationPermissions.AddAsync(newPermission);

                // Add to new permissions list
                newPermissions.Add(permissionEntity);
            }
        }

        await _context.SaveChangesAsync();

        // Return a new PermissionModel with the new permissions added or updated
        return new PermissionModel
        {
            CustomerId = permissionModel.CustomerId,
            Role = permissionModel.Role,
            CustomerEmail = permissionModel.CustomerEmail,
            RoleId = roleId,
            PermissionEntities = newPermissions
        };
    }


}
