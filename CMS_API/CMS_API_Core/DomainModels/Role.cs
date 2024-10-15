namespace CMS_API_Core.DomainModels
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = "Viewer";

        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
