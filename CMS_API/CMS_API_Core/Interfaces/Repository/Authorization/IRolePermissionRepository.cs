using CMS_API_Core.DomainModels.Authorization;

namespace CMS_API_Core.Interfaces.Repository.Authorization
{
    public interface IRolePermissionRepository
    {
        IEnumerable<RolePermission> GetRolePermissionByRoleId(string RolePermissionId);
        RolePermission GetRolePermissionByRoleIdANDPermissionId(string RoleId, string PermissionId);
        void AddRolePermission(RolePermission RolePermission);
        void DeleteRolePermission(string RolePermissionId);
        void SaveChanges();
        bool HasPermission(string RoleId, string PermissionName);
    }
}
