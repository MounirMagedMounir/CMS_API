using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.FilterModels.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Core.Interfaces.Repository.Article
{
    public interface ITagArticleRepository
    {
        IEnumerable<TagArticle> GetTagArticles(string artcleId);
        void AddTagArticle(TagArticle article);
        void DeleteTagArticle(string id);
        void SaveChanges();

    }
}
