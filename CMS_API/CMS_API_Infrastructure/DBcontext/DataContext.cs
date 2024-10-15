using CMS_API_Core.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_API_Infrastructure.DBcontext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
 
    }

}
