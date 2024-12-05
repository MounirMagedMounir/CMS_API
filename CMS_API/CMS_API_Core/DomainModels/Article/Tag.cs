namespace CMS_API_Core.DomainModels.Article
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<ContentArticle> Articles { get; set; }
    }
}
