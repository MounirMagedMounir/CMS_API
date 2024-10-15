using CMS_API_Core.Interfaces.Models;

namespace CMS_API_Core.DomainModels
{
    public class RolePermission : IBaseEntity
    {
        public string Id { get; set; }
        public string RoleId { get; set; } = "1";
        public string PermissionId { get; set; } = "1";
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
