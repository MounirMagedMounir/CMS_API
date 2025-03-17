using CMS_API_Application.Interfaces.Servises;
using CMS_API_Core.Interfaces.Repository.Authorization;
using Microsoft.Extensions.Logging;

namespace CMS_API_Infrastructure.Services
{
    public class SecurityService(IRolePermissionRepository roleRepository, ISetControllerNameToPermissionName setControllerNameToPermissionName) : ISecurityService
    {
        public bool HasPermissionByRole(Guid roleId, string permission)
        {
            setControllerNameToPermissionName.SetPermission(permission);
            var rolePermissions = roleRepository.GetRolePermissionByRoleId(roleId.ToString()).ToList();

            return rolePermissions.Any(rp => rp.Permission.Name == permission);
        }

    }
}
