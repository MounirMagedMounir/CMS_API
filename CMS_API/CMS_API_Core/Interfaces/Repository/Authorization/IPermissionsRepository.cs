using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.FilterModels;

namespace CMS_API_Core.Interfaces.Repository.Authorization
{
    public interface IPermissionRepository
    {
        bool IsPermissionExists(string permissionName);
        Permission GetPermissionById(string permissionId);
        IEnumerable<Permission> GetPermissions(PermissionsFilter filterParameter, string sortBy, string sortOrder);
        IEnumerable<Permission> GetPermissionsByName(string permissionName);
        IEnumerable<Permission> GetPermissionsNames();
        Permission GetPermissionByName(string permissionName);
        IEnumerable<Permission> GetAllPermissions();
        void AddPermission(Permission permission);
        void UpdatePermission(Permission permission);
        void DeletePermission(string permissionId);
        void SaveChanges();
    }
}
