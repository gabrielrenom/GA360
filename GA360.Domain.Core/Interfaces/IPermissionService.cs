using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.Domain.Core.Interfaces
{
    public interface IPermissionService
    {
        Task<List<Course>> FilterPermissions(string email, List<Course> courses);
        Task<List<Qualification>> FilterPermissions(string email, List<Qualification> qualifications);
        Task<List<Certificate>> FilterPermissions(string email, List<Certificate> certificates);
        Task<PermissionModel> GetPermissions(string email);
    }
}
