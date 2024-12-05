using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.FilterModels;
using CMS_API_Core.Interfaces.Repository.Authorization;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CMS_API_Infrastructure.Repository.Authorization
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetRoles(RolesFilter filterParameter, string sortBy, string sortOrder)
        {
            var query = _context.Role
                .Include(r => r.Permission)
                .Include(r => r.CreatedBy)
                .Include(r => r.LastUpdatedBy)
                .AsQueryable();

            // Filtering based on the `RolesFilter` filter
            if (Guid.TryParse(filterParameter.Id, out Guid parsedId) && parsedId != Guid.Empty)
            {
                query = query.Where(r => r.Id == parsedId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Name))
            {
                query = query.Where(r => r.Name.Contains(filterParameter.Name));
            }

            if (filterParameter.Permissions != null && filterParameter.Permissions.Any())
            {
                foreach (var permission in filterParameter.Permissions)
                {
                    if (!string.IsNullOrWhiteSpace(permission.Name))
                    {
                        query = query.Where(r => r.Permission.Any(rp => rp.Name == permission.Name));
                    }

                    if (Guid.TryParse(permission.Id, out Guid parsedPermissionId) && parsedPermissionId != Guid.Empty)
                    {
                        query = query.Where(r => r.Permission.Any(rp => rp.Id == parsedPermissionId.ToString()));
                    }
                }
            }

            if (Guid.TryParse(filterParameter.CreatedbyId, out Guid createdById) && createdById != Guid.Empty)
            {
                query = query.Where(r => r.CreatedbyId == createdById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.CreatedbyName))
            {
                query = query.Where(c => c.CreatedBy.UserName.Contains(filterParameter.CreatedbyName));
            }

            if (filterParameter.CreatedDateFrom.HasValue)
            {
                query = query.Where(r => r.CreatedDate.Date >= filterParameter.CreatedDateFrom.Value.Date);
            }

            if (filterParameter.CreatedDateTo.HasValue)
            {
                query = query.Where(r => r.CreatedDate.Date <= filterParameter.CreatedDateTo.Value.Date);
            }

            if (Guid.TryParse(filterParameter.LastUpdatedbyId, out Guid lastUpdatedById) && lastUpdatedById != Guid.Empty)
            {
                query = query.Where(r => r.LastUpdatedbyId == lastUpdatedById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.LastUpdatedbyName))
            {
                query = query.Where(c => c.LastUpdatedBy.UserName.Contains(filterParameter.LastUpdatedbyName));
            }

            if (filterParameter.LastUpdatedDateFrom.HasValue)
            {
                query = query.Where(r => r.LastUpdatedDate.Date >= filterParameter.LastUpdatedDateFrom.Value.Date);
            }

            if (filterParameter.LastUpdatedDateTo.HasValue)
            {
                query = query.Where(r => r.LastUpdatedDate.Date <= filterParameter.LastUpdatedDateTo.Value.Date);
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var propertyInfo = typeof(Role).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    var filterExpression = Expression.Parameter(typeof(Role), "r");
                    var propertyExpression = Expression.Property(filterExpression, propertyInfo);
                    var conversion = Expression.Convert(propertyExpression, typeof(object));
                    var lambda = Expression.Lambda<Func<Role, object>>(conversion, filterExpression);

                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(lambda)
                        : query.OrderBy(lambda);
                }
            }


            var roles = query.ToList();

            return roles;
        }

        public Role GetRoleByName(string roleName)
        {
            return _context.Role.Include(r => r.Permission).Include(r => r.CreatedBy).Include(r => r.LastUpdatedBy).FirstOrDefault(r => r.Name == roleName);
        }

        public Role GetRoleById(string roleId)
        {

            return _context.Role.Include(r => r.Permission).Include(r => r.CreatedBy).Include(r => r.LastUpdatedBy).FirstOrDefault(r => r.Id == roleId);
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

        public bool IsRoleNameExists(string rolename)
        {
            return _context.Role.Any(r => r.Name == rolename);
        }

        public void SaveChanges()
        {

            _context.SaveChanges();
        }

        public Role? GetRoleByIdOrName(string roleId, string roleName)
        {
            return _context.Role.FirstOrDefault(r => r.Id == roleId || r.Name == roleName);
        }
    }
}
