using CMS_API_Application.Interfaces.Servises;
using CMS_API_Core.Interfaces.Repository.Authorization;
using Microsoft.Extensions.Logging;

namespace CMS_API_Infrastructure.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IRolePermissionRepository _roleRepository;
    
        public SecurityService(IRolePermissionRepository roleRepository)
        {
            _roleRepository = roleRepository;
  
        }

        public bool HasPermissionByRole(Guid roleId, string permission)
        {

            var rolePermissions = _roleRepository.GetRolePermissionByRoleId(roleId.ToString()).ToList();
         
            return rolePermissions.Any(rp => rp.Permission.Name == permission);
        }
    }
}
