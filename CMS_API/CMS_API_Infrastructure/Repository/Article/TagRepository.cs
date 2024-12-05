using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.FilterModels.Article;
using CMS_API_Core.Interfaces.Repository.Article;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Infrastructure.Repository.Article
{
    public class TagRepository(DataContext _context) : ITagRepository
    {
        public void AddTag(Tag tag)
        {
            _context.Tag.Add(tag);
        }

        public void DeleteTag(string id)
        {
            _context.Tag.Remove(GetTagById(id));
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _context.Tag.Include(c => c.LastUpdatedBy).Include(c => c.CreatedBy).ToList();
        }
        public Tag GetTagById(string id)
        {
            return _context.Tag.Include(c => c.LastUpdatedBy).Include(c => c.CreatedBy).FirstOrDefault(t => t.Id == id);
        }

        public Tag GetTagByName(string tagName)
        {
            return _context.Tag.FirstOrDefault(t => t.Name == tagName);
        }

        public IEnumerable<Tag> GetTags(TagsFilter filterParameter, string sortBy, string sortOrder)
        {

            var query = _context.Tag.Include(c => c.LastUpdatedBy).Include(c => c.CreatedBy).AsQueryable();

            // Apply filters from UsersFilter class
            if (Guid.TryParse(filterParameter.Id, out Guid parsedId) && parsedId != Guid.Empty)
            {
                query = query.Where(t => t.Id == parsedId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Name))
            {
                query = query.Where(t => t.Name.Contains(filterParameter.Name));
            }

            if (Guid.TryParse(filterParameter.CreatedbyId, out Guid createdById) && createdById != Guid.Empty)
            {
                query = query.Where(t => t.CreatedbyId == createdById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.CreatedbyName))
            {
                query = query.Where(c => c.CreatedBy.UserName.Contains(filterParameter.CreatedbyName));
            }

            if (filterParameter.CreatedDateFrom.HasValue)
            {
                query = query.Where(t => t.CreatedDate.Date >= filterParameter.CreatedDateFrom.Value.Date);
            }

            if (filterParameter.CreatedDateTo.HasValue)
            {
                query = query.Where(t => t.CreatedDate.Date <= filterParameter.CreatedDateTo.Value.Date);
            }

            if (Guid.TryParse(filterParameter.LastUpdatedbyId, out Guid lastUpdatedById) && lastUpdatedById != Guid.Empty)
            {
                query = query.Where(t => t.LastUpdatedbyId == lastUpdatedById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.LastUpdatedbyName))
            {
                query = query.Where(c => c.LastUpdatedBy.UserName.Contains(filterParameter.LastUpdatedbyName));
            }

            if (filterParameter.LastUpdatedDateFrom.HasValue)
            {
                query = query.Where(t => t.LastUpdatedDate.Date >= filterParameter.LastUpdatedDateFrom.Value.Date);
            }

            if (filterParameter.LastUpdatedDateTo.HasValue)
            {
                query = query.Where(t => t.LastUpdatedDate.Date <= filterParameter.LastUpdatedDateTo.Value.Date);
            }

            // Apply sorting based on sortBy and sortOrder
            var propertyInfo = typeof(Tag).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                var exprParameter = Expression.Parameter(typeof(Tag), "u");
                var property = Expression.Property(exprParameter, propertyInfo);
                var conversion = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<Tag, object>>(conversion, exprParameter);

                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(lambda)
                    : query.OrderBy(lambda);
            }

            // Apply pagination
            return query.ToList();
        }
        public bool IsTagExists(string id)
        {
            return _context.Tag.Any(e => e.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateTag(Tag tag)
        {
            _context.Tag.Update(tag);
        }
    }
}
