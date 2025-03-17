using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.FilterModels;
using CMS_API_Core.Interfaces.Repository.Authorization;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

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

        public IEnumerable<Permission> GetPermissions(PermissionsFilter filterParameter, string sortBy, string sortOrder)
        {
            var query = _context.Permissions
                .Include(p => p.CreatedBy)
                .Include(p => p.LastUpdatedBy)
                .Include(p => p.Role)
                .AsQueryable();

            // Filtering based on the `PermissionsFilter` filter
            if (Guid.TryParse(filterParameter.Id, out Guid parsedId) && parsedId != Guid.Empty)
            {
                query = query.Where(p => p.Id == parsedId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Name))
            {
                query = query.Where(p => p.Name.Contains(filterParameter.Name));
            }

            if (filterParameter.Roles != null && filterParameter.Roles.Any())
            {
                foreach (var role in filterParameter.Roles)
                {
                    if (!string.IsNullOrWhiteSpace(role.Name))
                    {
                        query = query.Where(p => p.Role.Any(rp => rp.Name == role.Name));
                    }

                    if (Guid.TryParse(role.Id, out Guid parsedRoleId) && parsedRoleId != Guid.Empty)
                    {
                        query = query.Where(p => p.Role.Any(rp => rp.Id == parsedRoleId.ToString()));
                    }
                }
            }

            if (Guid.TryParse(filterParameter.CreatedbyId, out Guid createdById) && createdById != Guid.Empty)
            {
                query = query.Where(p => p.CreatedbyId == createdById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.CreatedbyName))
            {
                query = query.Where(c => c.CreatedBy.UserName.Contains(filterParameter.CreatedbyName));
            }

            if (filterParameter.CreatedDateFrom.HasValue)
            {
                query = query.Where(p => p.CreatedDate.Date >= filterParameter.CreatedDateFrom.Value.Date);
            }

            if (filterParameter.CreatedDateTo.HasValue)
            {
                query = query.Where(p => p.CreatedDate.Date <= filterParameter.CreatedDateTo.Value.Date);
            }

            if (Guid.TryParse(filterParameter.LastUpdatedbyId, out Guid lastUpdatedById) && lastUpdatedById != Guid.Empty)
            {
                query = query.Where(p => p.LastUpdatedbyId == lastUpdatedById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.LastUpdatedbyName))
            {
                query = query.Where(c => c.LastUpdatedBy.UserName.Contains(filterParameter.LastUpdatedbyName));
            }

            if (filterParameter.LastUpdatedDateFrom.HasValue)
            {
                query = query.Where(p => p.LastUpdatedDate.Date >= filterParameter.LastUpdatedDateFrom.Value.Date);
            }

            if (filterParameter.LastUpdatedDateTo.HasValue)
            {
                query = query.Where(p => p.LastUpdatedDate.Date <= filterParameter.LastUpdatedDateTo.Value.Date);
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var propertyInfo = typeof(Permission).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    var filterExpression = Expression.Parameter(typeof(Permission), "p");
                    var propertyExpression = Expression.Property(filterExpression, propertyInfo);
                    var conversion = Expression.Convert(propertyExpression, typeof(object));
                    var lambda = Expression.Lambda<Func<Permission, object>>(conversion, filterExpression);

                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(lambda)
                        : query.OrderBy(lambda);
                }
            }


            var permissions = query.ToList();

            return permissions;
        }

        public Permission? GetPermissionByName(string permissionName)
        {
            return _context.Permissions.Include(p => p.CreatedBy).Include(p => p.LastUpdatedBy).FirstOrDefault(p => p.Name.Contains(permissionName));
        }

        public IEnumerable<Permission> GetPermissionsByName(string permissionName)
        {
            return _context.Permissions.Include(p => p.CreatedBy).Include(p => p.LastUpdatedBy).Where(p => p.Name.Contains(permissionName)).ToList();
        }

        IEnumerable<Permission> IPermissionRepository.GetPermissionsNames()
        {
            return _context.Permissions.ToList();

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
