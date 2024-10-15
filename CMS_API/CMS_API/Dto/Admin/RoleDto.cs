using CMS_API_Core.DomainModels;

namespace CMS_API.Dto.Admin
{
    public class RoleDto
    {
        public string? Id { get; set; }

        public string Name { get; set; } = "Viewer";

        public virtual ICollection<PermissionDto> Permissions { get; set; }
    }
}
