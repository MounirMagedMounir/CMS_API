namespace CMS_API_Core.DomainModels.Authorization
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = "Viewer";
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Permission> Permission { get; set; }
    }
}
