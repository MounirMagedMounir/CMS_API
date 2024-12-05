using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.Interfaces.Repository.Article;
using CMS_API_Infrastructure.DBcontext;

namespace CMS_API_Infrastructure.Repository.Article
{
    public class TagArticleRepository(DataContext _context) : ITagArticleRepository
    {
        public void AddTagArticle(TagArticle article)
        {
            _context.TagArticle.Add(article);
        }

        public void DeleteTagArticle(string id)
        {
            var tagArticle = _context.TagArticle.FirstOrDefault(ta => ta.Id == id);
            if (tagArticle != null)
            {
                _context.TagArticle.Remove(tagArticle);
            }

        }

        public IEnumerable<TagArticle> GetTagArticles(string artcleId)
        {
     return _context.TagArticle.Where(ta => ta.ArticleId == artcleId).ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
