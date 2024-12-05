using CMS_API_Core.DomainModels;
using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.FilterModels.Article;
using CMS_API_Core.Interfaces.Repository.Article;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CMS_API_Infrastructure.Repository.Article
{
    public class ArticleRepository(DataContext _context) : IArticleRepository
    {

        public void CreateArticle(ContentArticle article)
        {
            _context.Article.Add(article);
        }

        public void DeleteArticle(string id)
        {
            _context.Article.Remove(GetArticleById(id));
        }

        public ContentArticle GetArticleById(string id)
        {
            return _context.Article.Include(a => a.Tags).Include(c => c.CreatedBy).Include(c => c.LastUpdatedBy).FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<ContentArticle> GetArticles(ArticleFilter filterParameter, string sortBy, string sortOrder)
        {
            var query = _context.Article.Include(a => a.Tags).Include(c => c.CreatedBy).Include(c => c.LastUpdatedBy).AsQueryable();

            // Apply filters from UsersFilter class
            if (Guid.TryParse(filterParameter.Id, out Guid parsedId) && parsedId != Guid.Empty)
            {
                query = query.Where(a => a.Id == parsedId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Name))
            {
                query = query.Where(a => a.Name.Contains(filterParameter.Name));
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Title))
            {
                query = query.Where(a => a.Title.Contains(filterParameter.Title));
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Description))
            {
                query = query.Where(a => a.Description.Contains(filterParameter.Description));
            }

            if (filterParameter.Status.HasValue)
            {
                query = query.Where(a => a.Status == filterParameter.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Content))
            {
                query = query.Where(a => a.Content.Contains(filterParameter.Content));
            }

            if (filterParameter.PublishDate.HasValue)
            {
                query = query.Where(a => a.PublishDate.Value.Date >= filterParameter.PublishDate.Value.Date);
            }
            if (filterParameter.ViewsCount > 0)
            {
                query = query.Where(a => a.ViewsCount == filterParameter.ViewsCount);
            }

            if (filterParameter.Tags != null && filterParameter.Tags.Any())
            {
                foreach (var tag in filterParameter.Tags)
                {
                    if (!string.IsNullOrWhiteSpace(tag))
                    {
                        query = query.Where(a => a.Tags.Any(t => t.Name == tag));
                    }
                }
            }

            if (Guid.TryParse(filterParameter.CreatedbyId, out Guid createdById) && createdById != Guid.Empty)
            {
                query = query.Where(a => a.CreatedbyId == createdById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.CreatedbyName))
            {
                query = query.Where(c => c.CreatedBy.UserName.Contains(filterParameter.CreatedbyName));
            }

            if (filterParameter.CreatedDateFrom.HasValue)
            {
                query = query.Where(a => a.CreatedDate.Date >= filterParameter.CreatedDateFrom.Value.Date);
            }

            if (filterParameter.CreatedDateTo.HasValue)
            {
                query = query.Where(a => a.CreatedDate.Date <= filterParameter.CreatedDateTo.Value.Date);
            }

            if (Guid.TryParse(filterParameter.LastUpdatedbyId, out Guid lastUpdatedById) && lastUpdatedById != Guid.Empty)
            {
                query = query.Where(a => a.LastUpdatedbyId == lastUpdatedById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.LastUpdatedbyName))
            {
                query = query.Where(c => c.LastUpdatedBy.UserName.Contains(filterParameter.LastUpdatedbyName));
            }

            if (filterParameter.LastUpdatedDateFrom.HasValue)
            {
                query = query.Where(a => a.LastUpdatedDate.Date >= filterParameter.LastUpdatedDateFrom.Value.Date);
            }

            if (filterParameter.LastUpdatedDateTo.HasValue)
            {
                query = query.Where(a => a.LastUpdatedDate.Date <= filterParameter.LastUpdatedDateTo.Value.Date);
            }

            // Apply sorting based on sortBy and sortOrder
            var propertyInfo = typeof(ContentArticle).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                var exprParameter = Expression.Parameter(typeof(ContentArticle), "u");
                var property = Expression.Property(exprParameter, propertyInfo);
                var conversion = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<ContentArticle, object>>(conversion, exprParameter);

                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(lambda)
                    : query.OrderBy(lambda);
            }

            // Apply pagination
            return query.ToList();
        }

        public IEnumerable<ContentArticle> GetArticlesByUserId(string UserId, string sortBy, string sortOrder)
        {
            var query = _context.Article.Include(a => a.Tags).Include(c => c.CreatedBy).Include(c => c.LastUpdatedBy).Where(a => a.CreatedbyId == UserId).AsQueryable();
            // Apply sorting based on sortBy and sortOrder
            var propertyInfo = typeof(ContentArticle).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                var exprParameter = Expression.Parameter(typeof(ContentArticle), "u");
                var property = Expression.Property(exprParameter, propertyInfo);
                var conversion = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<ContentArticle, object>>(conversion, exprParameter);

                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(lambda)
                    : query.OrderBy(lambda);
            }

            // Apply pagination
            return query.ToList();
        }

        public bool IsArticleExists(string id)
        {
            return _context.Article.Any(e => e.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateArticle(ContentArticle article)
        {
            _context.Article.Update(article);
        }
    }

}
