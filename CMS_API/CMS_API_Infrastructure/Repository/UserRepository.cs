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

        public IEnumerable<User> GetUsers(UsersFilter? filterParameter, string? sortBy, string? sortOrder)
        {
            var query = _context.User.Include(u => u.Role).Include(c => c.CreatedBy).Include(c => c.LastUpdatedBy).AsQueryable();

            // Apply filters from UsersFilter class
            if (Guid.TryParse(filterParameter.Id, out Guid parsedId) && parsedId != Guid.Empty)
            {
                query = query.Where(u => u.Id == parsedId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Name))
            {
                query = query.Where(u => u.Name.Contains(filterParameter.Name));
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Email))
            {
                query = query.Where(u => u.Email.Contains(filterParameter.Email));
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.UserName))
            {
                query = query.Where(u => u.UserName.Contains(filterParameter.UserName));
            }

            if (filterParameter.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == filterParameter.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Phone))
            {
                query = query.Where(u => u.Phone.Contains(filterParameter.Phone));
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.RoleName))
            {
                query = query.Where(u => u.Role.Name.Contains(filterParameter.RoleName));
            }

            if (Guid.TryParse(filterParameter.CreatedbyId, out Guid createdById) && createdById != Guid.Empty)
            {
                query = query.Where(u => u.CreatedbyId == createdById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.CreatedbyName))
            {
                query = query.Where(c => c.CreatedBy.UserName.Contains(filterParameter.CreatedbyName));
            }

            if (filterParameter.CreatedDateFrom.HasValue)
            {
                query = query.Where(u => u.CreatedDate.Date >= filterParameter.CreatedDateFrom.Value.Date);
            }

            if (filterParameter.CreatedDateTo.HasValue)
            {
                query = query.Where(u => u.CreatedDate.Date <= filterParameter.CreatedDateTo.Value.Date);
            }

            if (Guid.TryParse(filterParameter.LastUpdatedbyId, out Guid lastUpdatedById) && lastUpdatedById != Guid.Empty)
            {
                query = query.Where(u => u.LastUpdatedbyId == lastUpdatedById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.LastUpdatedbyName))
            {
                query = query.Where(c => c.LastUpdatedBy.UserName.Contains(filterParameter.LastUpdatedbyName));
            }

            if (filterParameter.LastUpdatedDateFrom.HasValue)
            {
                query = query.Where(u => u.LastUpdatedDate.Date >= filterParameter.LastUpdatedDateFrom.Value.Date);
            }

            if (filterParameter.LastUpdatedDateTo.HasValue)
            {
                query = query.Where(u => u.LastUpdatedDate.Date <= filterParameter.LastUpdatedDateTo.Value.Date);
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

        public void SaveChanges()
        {

            _context.SaveChanges();
        }

        public IEnumerable<User> GetUsersByParameter(string parameter)
        {
            return _context.User.Include(u => u.Role)
                .Include(c => c.CreatedBy)
                .Include(c => c.LastUpdatedBy)
                .Where(u => EF.Functions.Like(u.Id, $"%{parameter}%") ||
                            EF.Functions.Like(u.Name, $"%{parameter}%") ||
                            EF.Functions.Like(u.Email, $"%{parameter}%") ||
                            EF.Functions.Like(u.UserName, $"%{parameter}%") ||
                            EF.Functions.Like(u.IsActive.ToString(), $"%{parameter}%") ||
                            EF.Functions.Like(u.Phone, $"%{parameter}%") ||
                            EF.Functions.Like(u.Role.Name, $"%{parameter}%") ||
                            EF.Functions.Like(u.CreatedbyId, $"%{parameter}%") ||
                            EF.Functions.Like(u.CreatedBy.UserName, $"%{parameter}%") ||
                            EF.Functions.Like(u.CreatedDate.ToString(), $"%{parameter}%") ||
                            EF.Functions.Like(u.LastUpdatedbyId, $"%{parameter}%") ||
                            EF.Functions.Like(u.LastUpdatedBy.UserName, $"%{parameter}%") ||
                            EF.Functions.Like(u.LastUpdatedDate.ToString(), $"%{parameter}%")
                           ).ToList();
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _context.User.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public bool IsUserRoleAdmin(string userId)
        {
            return _context.User.Include(u => u.Role).Any(u => u.Id == userId && u.Role.Name == "Admin" || u.Role.Name == "Super Admin");
        }
    }
}
