using CMS_API_Core.DomainModels;
using CMS_API_Core.FilterModels;


namespace CMS_API_Core.Interfaces.Repository
{
    public interface IRoleRepository
    {
        Role GetRoleById(string roleId);
        Role GetRoleByIdOrName(string roleId,string roleName);
        IEnumerable<Role> GetRoles(RolesFilter filter, string sortBy, string sortOrder);
        IEnumerable<Role> GetRolesByName(string roleName);
        Role GetRoleByName(string roleName);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(string roleId);
        bool IsRoleExists(string rolename);
        Task SaveChangesAsync();
    }
}
