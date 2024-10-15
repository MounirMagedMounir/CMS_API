using CMS_API_Core.DomainModels;
using CMS_API_Core.FilterModels;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CMS_API_Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetRoles(RolesFilter parameter, string sortBy, string sortOrder)
        {
            var query = _context.Role
                .Include(r => r.RolePermission)
                .ThenInclude(rp => rp.Permission)
                .AsQueryable();

            // Filtering based on the `RolesFilter` parameter
            if (Guid.TryParse(parameter.Id, out Guid parsedId) && parsedId != Guid.Empty)
            {
                query = query.Where(r => r.Id == parsedId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(parameter.Name))
            {
                query = query.Where(r => r.Name.Contains(parameter.Name));
            }

            if (parameter.Permissions != null && parameter.Permissions.Any())
            {
                foreach (var permission in parameter.Permissions)
                {
                    if (!string.IsNullOrWhiteSpace(permission.Name))
                    {
                        query = query.Where(r => r.RolePermission.Any(rp => rp.Permission.Name == permission.Name));
                    }

                    if (Guid.TryParse(permission.Id, out Guid parsedPermissionId) && parsedPermissionId != Guid.Empty)
                    {
                        query = query.Where(r => r.RolePermission.Any(rp => rp.Permission.Id == parsedPermissionId.ToString()));
                    }
                }
            }

            if (Guid.TryParse(parameter.CreatedbyId, out Guid createdById) && createdById != Guid.Empty)
            {
                query = query.Where(r => r.CreatedbyId == createdById.ToString());
            }

            if (parameter.CreatedDateFrom.HasValue)
            {
                query = query.Where(r => r.CreatedDate.Date >= parameter.CreatedDateFrom.Value.Date);
            }

            if (parameter.CreatedDateTo.HasValue)
            {
                query = query.Where(r => r.CreatedDate.Date <= parameter.CreatedDateTo.Value.Date);
            }

            if (Guid.TryParse(parameter.LastUpdatedbyId, out Guid lastUpdatedById) && lastUpdatedById != Guid.Empty)
            {
                query = query.Where(r => r.LastUpdatedbyId == lastUpdatedById.ToString());
            }

            if (parameter.LastUpdatedDateFrom.HasValue)
            {
                query = query.Where(r => r.LastUpdatedDate.Date >= parameter.LastUpdatedDateFrom.Value.Date);
            }

            if (parameter.LastUpdatedDateTo.HasValue)
            {
                query = query.Where(r => r.LastUpdatedDate.Date <= parameter.LastUpdatedDateTo.Value.Date);
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var propertyInfo = typeof(Role).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    var parameterExpression = Expression.Parameter(typeof(Role), "r");
                    var propertyExpression = Expression.Property(parameterExpression, propertyInfo);
                    var conversion = Expression.Convert(propertyExpression, typeof(object));
                    var lambda = Expression.Lambda<Func<Role, object>>(conversion, parameterExpression);

                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(lambda)
                        : query.OrderBy(lambda);
                }
            }

            // Pagination
            var roles = query.ToList();

            return roles;
        }

        public IEnumerable<Role> GetRolesByName(string roleName)
        {
            return _context.Role.Where(r => r.Name == roleName);
        }

        public Role GetRoleByName(string roleName)
        {
            return _context.Role.FirstOrDefault(r => r.Name == roleName);
        }

        public Role GetRoleById(string roleId)
        {
            Console.WriteLine("GetRoleById  " + roleId);
            return _context.Role.Include(r => r.RolePermission).ThenInclude(rp => rp.Permission).FirstOrDefault(r => r.Id == roleId);
        }

        public void AddRole(Role role)
        {
            _context.Role.Add(role);
        }

        public void UpdateRole(Role role)
        {
            _context.Role.Update(role);
        }

        public void DeleteRole(string roleId)
        {
            var role = _context.Role.FirstOrDefault(r => r.Id == roleId);
            if (role != null)
            {
                _context.Role.Remove(role);
            }
        }

        public bool IsRoleExists(string rolename)
        {
            return _context.Role.Any(r => r.Name == rolename);
        }

        public async Task SaveChangesAsync()
        {

            _context.SaveChanges();
        }

        public Role? GetRoleByIdOrName(string roleId, string roleName)
        {
            return _context.Role.FirstOrDefault(r => r.Id == roleId || r.Name == roleName);
        }
    }
}
