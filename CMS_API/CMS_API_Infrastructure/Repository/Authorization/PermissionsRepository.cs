using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.Interfaces.Repository.Authorization;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CMS_API_Infrastructure.Repository.Authorization
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
            return _context.Permissions.Include(p => p.CreatedBy).Include(p => p.LastUpdatedBy).FirstOrDefault(p => p.Id == permissionId);
        }

        public IEnumerable<Permission> GetPermissionsById(string permissionId)
        {
            return _context.Permissions.Include(p => p.CreatedBy).Include(p => p.LastUpdatedBy).Where(p => p.Id == permissionId).ToList();
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return _context.Permissions.Include(p => p.CreatedBy).Include(p => p.LastUpdatedBy).ToList();
        }

        public Permission? GetPermissionByName(string permissionName)
        {
            return _context.Permissions.Include(p => p.CreatedBy).Include(p => p.LastUpdatedBy).FirstOrDefault(p => p.Name.Contains(permissionName));
        }

        public IEnumerable<Permission> GetPermissionsByName(string permissionName)
        {
            return _context.Permissions.Include(p => p.CreatedBy).Include(p => p.LastUpdatedBy).Where(p => p.Name.Contains(permissionName)).ToList();
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

        public void SaveChanges()
        {

            _context.SaveChanges();
        }


        public IEnumerable<Permission> GetAllPermissions()
        {
            return _context.Permissions.Include(p => p.CreatedBy).Include(p => p.LastUpdatedBy).ToList();
        }




    }
}
