namespace CMS_API_Application.Dto.Authorization
{
    public class GetRoleDto : BaseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<GetRolePermissionDto>? Permissions { get; set; }
    }
}
