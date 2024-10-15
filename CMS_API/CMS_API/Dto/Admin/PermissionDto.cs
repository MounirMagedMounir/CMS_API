using CMS_API_Core.DomainModels;

namespace CMS_API.Dto.Admin
{
    public class PermissionDto
    {
        public string? Id { get; set; }

        public string Name { get; set; } = "Viewer";

    }
}
