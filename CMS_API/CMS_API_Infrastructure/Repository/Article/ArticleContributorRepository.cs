using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.Interfaces.Repository.Article;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Infrastructure.Repository.Article
{
    public class ArticleContributorRepository(DataContext _context) : IArticleContributorRepository
    {
        public void AddArticleContributor(ArticleContributor article)
        {
            _context.ArticleContributor.Add(article);
        }

        public void DeleteArticleContributor(string id)
        {

            _context.ArticleContributor.Remove(GetArticleContributorById(id));

        }

        public ArticleContributor GetArticleContributorById(string id)
        {
            return _context.ArticleContributor.Include(ac => ac.User).Include(ac => ac.Article).FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<ArticleContributor> GetArticleContributorsByArticleId(string articleId)
        {
            return _context.ArticleContributor.Include(ac => ac.User).Include(ac => ac.Article).Where(a => a.ArticleId == articleId).ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateArticleContributor(ArticleContributor article)
        {
            _context.ArticleContributor.Update(article);
        }
    }
}
