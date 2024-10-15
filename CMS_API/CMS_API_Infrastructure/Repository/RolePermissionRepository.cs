using CMS_API_Core.DomainModels;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Infrastructure.DBcontext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Infrastructure.Repository
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly DataContext _context;

        public RolePermissionRepository(DataContext context)
        {
            _context = context;
        }

        public void AddRolePermission(RolePermission RolePermission)
        {
            _context.RolePermission.Add(RolePermission);
        }

        public void DeleteRolePermission(string RolePermissionId)
        {
            var rolepermission = _context.RolePermission.FirstOrDefault(rp => rp.Id == RolePermissionId);
            if (rolepermission != null)
            {
                _context.RolePermission.Remove(rolepermission);
            }
        }

        public IEnumerable<RolePermission> GetRolePermissionByRoleId(string RolePermissionId)
        {
            return _context.RolePermission.Where(rp => rp.RoleId == RolePermissionId).ToList();
        }

        public async Task SaveChangesAsync()
        {

            _context.SaveChanges();
        }


    }
}
