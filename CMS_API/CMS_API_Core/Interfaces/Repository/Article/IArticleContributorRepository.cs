using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.FilterModels.Article;

namespace CMS_API_Core.Interfaces.Repository.Article
{
    public interface IArticleContributorRepository
    {
        IEnumerable<ArticleContributor> GetArticleContributorsByArticleId(string articleId);
        ArticleContributor GetArticleContributorById(string id);
        void AddArticleContributor(ArticleContributor article);
        void UpdateArticleContributor(ArticleContributor article);
        void DeleteArticleContributor(string id);
        void SaveChanges();
    }
}
