using CMS_API_Core.enums;

namespace CMS_API_Core.DomainModels.Article
{
    public class ContentArticle : BaseEntity
    {

        public string Name { get; set; }
        public string? Title { get; set; }
        public DateTime? PublishDate { get; set; } = null;
        public ArticleStatus Status { get; set; } = ArticleStatus.Draft;
        public int ViewsCount { get; set; } = 0;
        public string? Description { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
        public string? Video { get; set; }
        public virtual ICollection<Comment?>? Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<User> Contributors { get; set; }


    }
}
