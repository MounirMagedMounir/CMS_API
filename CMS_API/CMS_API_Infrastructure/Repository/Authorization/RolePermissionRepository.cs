using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.Interfaces.Repository.Authorization;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Infrastructure.Repository.Authorization
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RolePermissionRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
            return _context.RolePermission.Include(rp => rp.Permission).Where(rp => rp.RoleId == RolePermissionId).ToList();
        }
        public RolePermission GetRolePermissionByRoleIdANDPermissionId(string RoleId, string PermissionId)
        {
            return _context.RolePermission.Include(rp => rp.Permission).FirstOrDefault(rp => rp.RoleId == RoleId && rp.PermissionId == PermissionId);
        }
        public void SaveChanges()
        {

            _context.SaveChanges();
        }
        public bool HasPermission(string RoleId, string PermissionName)
        {
            var rolepermission = _context.RolePermission.Include(rp => rp.Permission).FirstOrDefault(rp => rp.RoleId == RoleId && rp.Permission.Name == PermissionName);
            if (rolepermission is not null)
            {
                return true;
            }
            return false;
        }
    }
}
