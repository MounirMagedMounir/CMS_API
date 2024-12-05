using CMS_API_Core.enums;

namespace CMS_API_Core.DomainModels.Article
{
    public class ArticleContributor
    {
        public string Id { get; set; }
        public string ArticleId { get; set; }
        public string UserId { get; set; }

        public ContentArticle Article { get; set; }
        public User User { get; set; }
        public ContributorRole ContributorRole { get; set; } // Specifies role for this contributor
    }

}
