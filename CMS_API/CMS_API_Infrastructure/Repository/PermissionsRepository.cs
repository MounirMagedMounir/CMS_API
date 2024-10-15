using CMS_API_Core.DomainModels;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;

namespace CMS_API_Infrastructure.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DataContext _context;

        public PermissionRepository(DataContext context)
        {
            _context = context;
        }

        public bool IsPermissionExists(string permissionName)
        {
            return _context.Permissions.Any(p => p.Name == permissionName);
        }

        public Permission? GetPermissionById(string permissionId)
        {
            return _context.Permissions.FirstOrDefault(p => p.Id == permissionId);
        }
        public IEnumerable<Permission> GetPermissionsById(string permissionId)
        {
            return _context.Permissions.Where(p => p.Id == permissionId).ToList();
        }

        public  IEnumerable<Permission> GetPermissions()
        {
            return  _context.Permissions.ToList();
        }

        public async Task<Permission?> GetPermissionByName(string permissionName)
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name.Contains(permissionName));
        }

        public IEnumerable<Permission> GetPermissionsByName(string permissionName)
        {
            return _context.Permissions.Where(p => p.Name.Contains(permissionName)).ToList();
        }

        public void AddPermission(Permission permission)
        {
            _context.Permissions.Add(permission);
        }

        public void UpdatePermission(Permission permission)
        {
            _context.Permissions.Update(permission);
        }

        public void DeletePermission(string permissionId)
        {
            var permission = GetPermissionById(permissionId);
            if (permission != null)
            {
                _context.Permissions.Remove(permission);
            }
        }

        public async Task SaveChangesAsync()
        {

            _context.SaveChanges();
        }


        public IEnumerable<Permission> GetAllPermissions()
        {
            return _context.Permissions.ToList();
        }
    }
}
