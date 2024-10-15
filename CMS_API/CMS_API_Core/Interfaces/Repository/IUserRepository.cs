using CMS_API_Core.DomainModels;
using CMS_API_Core.FilterModels;

namespace CMS_API_Core.Interfaces.Repository
{
    public interface IUserRepository
    {
        User GetUserById(string userId);
        User GetUserByEmailAndPassword(string email, string password);
        IEnumerable<User> GetUsers(UsersFilter filter, string sortBy, string sortOrder);
        IEnumerable<User> GetUsersByName(string userName);
        IEnumerable<User> GetUsersByRole(string roleName);
        IEnumerable<User> GetUsersByParameter(string parameter);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(string userId);
        void DeleteUsers(List<string> userIds);
        bool IsEmailExists(string email);
        Task SaveChangesAsync();
    }

}
