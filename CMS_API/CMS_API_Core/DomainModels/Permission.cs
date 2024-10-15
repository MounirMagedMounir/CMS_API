namespace CMS_API_Core.DomainModels
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; } = "View";

        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
