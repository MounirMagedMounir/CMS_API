using CMS_API_Core.Interfaces.Models;

namespace CMS_API_Core.DomainModels.Authorization
{
    public class RolePermission : IBaseEntity
    {
        public string Id { get; set; } // Primary Key
        public string RoleId { get; set; } // Foreign Key to Role
        public string PermissionId { get; set; } // Foreign Key to Permission
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
