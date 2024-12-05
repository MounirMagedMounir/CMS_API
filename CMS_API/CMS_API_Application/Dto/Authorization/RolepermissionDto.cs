namespace CMS_API_Application.Dto.Authorization
{
    public class RolepermissionDto
    {
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public List<string>? PermissionsId { get; set; }
        public List<string>? PermissionsName { get; set; }

    }
}
