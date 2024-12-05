namespace CMS_API_Core.DomainModels.Authorization
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; } = "View";

        public virtual ICollection<Role> Role { get; set; }
    }
}
