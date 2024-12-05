using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.FilterModels;


namespace CMS_API_Core.Interfaces.Repository.Authorization
{
    public interface IRoleRepository
    {
        Role GetRoleById(string roleId);
        Role GetRoleByIdOrName(string roleId, string roleName);
        IEnumerable<Role> GetRoles(RolesFilter filterParameter, string sortBy, string sortOrder);
        Role GetRoleByName(string roleName);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(string roleId);
        bool IsRoleNameExists(string rolename);
        void SaveChanges();
    }
}
