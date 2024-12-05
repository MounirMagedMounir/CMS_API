using CMS_API_Core.FilterModels.Article;
using CMS_API_Core.DomainModels.Article;

namespace CMS_API_Core.Interfaces.Repository.Article
{
    public interface IArticleRepository
    {
        IEnumerable<ContentArticle> GetArticles(ArticleFilter filterParameter, string sortBy, string sortOrder);
        IEnumerable<ContentArticle> GetArticlesByUserId(string UserId, string sortBy, string sortOrder);
        ContentArticle GetArticleById(string id);
        void CreateArticle(ContentArticle article);
        void UpdateArticle(ContentArticle article);
        void DeleteArticle(string id);
        bool IsArticleExists(string id);
        void SaveChanges();
    }
}
