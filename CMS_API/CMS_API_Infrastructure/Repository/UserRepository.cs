using CMS_API_Core.DomainModels;
using CMS_API_Core.FilterModels;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CMS_API_Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User GetUserById(string userId)
        {
            return _context.User.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);
        }

        public IEnumerable<User> GetUsers(UsersFilter filter, string sortBy, string sortOrder)
        {
            var query = _context.User.Include(u => u.Role).AsQueryable();

            // Apply filters from UsersFilter class
            if (Guid.TryParse(filter.Id, out Guid parsedId) && parsedId != Guid.Empty)
            {
                query = query.Where(u => u.Id == parsedId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(u => u.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(u => u.Email.Contains(filter.Email));
            }

            if (!string.IsNullOrWhiteSpace(filter.UserName))
            {
                query = query.Where(u => u.UserName.Contains(filter.UserName));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == filter.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Phone))
            {
                query = query.Where(u => u.Phone.Contains(filter.Phone));
            }

            if (!string.IsNullOrWhiteSpace(filter.RoleName))
            {
                query = query.Where(u => u.Role.Name.Contains(filter.RoleName));
            }

            if (Guid.TryParse(filter.CreatedbyId, out Guid createdById) && createdById != Guid.Empty)
            {
                query = query.Where(u => u.CreatedbyId == createdById.ToString());
            }

            if (filter.CreatedDateFrom.HasValue)
            {
                query = query.Where(u => u.CreatedDate.Date >= filter.CreatedDateFrom.Value.Date);
            }

            if (filter.CreatedDateTo.HasValue)
            {
                query = query.Where(u => u.CreatedDate.Date <= filter.CreatedDateTo.Value.Date);
            }

            if (Guid.TryParse(filter.LastUpdatedbyId, out Guid lastUpdatedById) && lastUpdatedById != Guid.Empty)
            {
                query = query.Where(u => u.LastUpdatedbyId == lastUpdatedById.ToString());
            }

            if (filter.LastUpdatedDateFrom.HasValue)
            {
                query = query.Where(u => u.LastUpdatedDate.Date >= filter.LastUpdatedDateFrom.Value.Date);
            }

            if (filter.LastUpdatedDateTo.HasValue)
            {
                query = query.Where(u => u.LastUpdatedDate.Date <= filter.LastUpdatedDateTo.Value.Date);
            }

            // Apply sorting based on sortBy and sortOrder
            var propertyInfo = typeof(User).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                var exprParameter = Expression.Parameter(typeof(User), "u");
                var property = Expression.Property(exprParameter, propertyInfo);
                var conversion = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<User, object>>(conversion, exprParameter);

                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(lambda)
                    : query.OrderBy(lambda);
            }

            // Apply pagination
            return query.ToList();
        }

        public IEnumerable<User> GetUsersByName(string userName)
        {
            return _context.User.Where(u => u.Name.Contains(userName));
        }

        public IEnumerable<User> GetUsersByRole(string roleName)
        {
            var role = _context.Role.FirstOrDefault(r => r.Name == roleName);
            return role != null ? _context.User.Where(u => u.RoleId == role.Id) : null;
        }

        public void AddUser(User user)
        {
            _context.User.Add(user);
        }

        public void UpdateUser(User user)
        {
         
            _context.User.Update(user);
        }

        public void DeleteUser(string userId)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                _context.User.Remove(user);
            }
        }

        public void DeleteUsers(List<string> userIds)
        {
            var users = _context.User.Where(u => userIds.Contains(u.Id));
            _context.User.RemoveRange(users);
        }

        public bool IsEmailExists(string email)
        {
            return _context.User.Any(u => u.Email == email);
        }

        public async Task SaveChangesAsync()
        {
           
            _context.SaveChanges();
        }

        public IEnumerable<User> GetUsersByParameter(string parameter)
        {
            return _context.User.Include(u => u.Role)
                .Where(u => EF.Functions.Like(u.Id, $"%{parameter}%") ||
                            EF.Functions.Like(u.Name, $"%{parameter}%") ||
                            EF.Functions.Like(u.Email, $"%{parameter}%") ||
                            EF.Functions.Like(u.UserName, $"%{parameter}%") ||
                            EF.Functions.Like(u.IsActive.ToString(), $"%{parameter}%") ||
                            EF.Functions.Like(u.Phone, $"%{parameter}%") ||
                            EF.Functions.Like(u.Role.Name, $"%{parameter}%") ||
                            EF.Functions.Like(u.CreatedbyId, $"%{parameter}%") ||
                            EF.Functions.Like(u.CreatedDate.ToString(), $"%{parameter}%") ||
                            EF.Functions.Like(u.LastUpdatedbyId, $"%{parameter}%") ||
                            EF.Functions.Like(u.LastUpdatedDate.ToString(), $"%{parameter}%"))
                .ToList();
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _context.User.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
