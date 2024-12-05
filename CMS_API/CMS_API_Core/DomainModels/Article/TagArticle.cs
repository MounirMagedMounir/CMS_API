using CMS_API_Core.Interfaces.Models;

namespace CMS_API_Core.DomainModels.Article
{
    public class TagArticle : IBaseEntity
    {
        public string Id { get; set; }
        public string TagId { get; set; }
        public string ArticleId { get; set; }
        public Tag Tag { get; set; }
        public ContentArticle Article { get; set; }
    }
}
