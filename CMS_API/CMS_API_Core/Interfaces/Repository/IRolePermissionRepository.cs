using CMS_API_Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Core.Interfaces.Repository
{
    public interface IRolePermissionRepository
    {
        IEnumerable<RolePermission> GetRolePermissionByRoleId(string RolePermissionId);
        void AddRolePermission(RolePermission RolePermission);
        void DeleteRolePermission(string RolePermissionId);
        Task SaveChangesAsync();
    }
}
