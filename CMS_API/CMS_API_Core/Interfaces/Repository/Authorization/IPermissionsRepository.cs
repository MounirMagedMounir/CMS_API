using CMS_API_Core.DomainModels.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Core.Interfaces.Repository.Authorization
{
    public interface IPermissionRepository
    {
        bool IsPermissionExists(string permissionName);
        Permission GetPermissionById(string permissionId);
        IEnumerable<Permission> GetPermissions();
        IEnumerable<Permission> GetPermissionsByName(string permissionName);
        Permission GetPermissionByName(string permissionName);
        IEnumerable<Permission> GetAllPermissions();
        void AddPermission(Permission permission);
        void UpdatePermission(Permission permission);
        void DeletePermission(string permissionId);
        void SaveChanges();
    }
}
