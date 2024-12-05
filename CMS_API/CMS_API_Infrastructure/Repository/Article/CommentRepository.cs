using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.FilterModels.Article;
using CMS_API_Core.Interfaces.Repository.Article;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CMS_API_Infrastructure.Repository.Article
{
    public class CommentRepository(DataContext _context) : ICommentRepository
    {
        public void AddComment(Comment comment)
        {
            _context.Comment.Add(comment);
        }

        public void DeleteComment(string id)
        {
            // Get all child comments of the specified comment
            var childComments = _context.Comment.Where(c => c.ParentId == id).ToList();

            foreach (var child in childComments)
            {
                // Recursively delete each child comment
                DeleteComment(child.Id);
            }

            // Now delete the parent comment itself
            var comment = GetComment(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }
        }
        public IEnumerable<Comment> GetComments(CommentsFilter filterParameter, string sortBy, string sortOrder)
        {
            // Base query for filtering comments
            var query = _context.Comment.Include(c => c.CreatedBy).Include(c => c.LastUpdatedBy).AsQueryable();

            // Apply filters from CommentsFilter
            if (Guid.TryParse(filterParameter.Id, out Guid parsedId) && parsedId != Guid.Empty)
            {
                query = query.Where(c => c.Id == parsedId.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.Content))
            {
                query = query.Where(c => c.Content.Contains(filterParameter.Content));
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.ApprovedById))
            {
                query = query.Where(c => c.ApprovedById.Contains(filterParameter.ApprovedById));
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.ArticleId))
            {
                query = query.Where(c => c.ArticleId.Contains(filterParameter.ArticleId));
            }

            if (filterParameter.IsApproved.HasValue)
            {
                query = query.Where(c => c.IsApproved == filterParameter.IsApproved.Value);
            }

            if (filterParameter.ApprovedDate.HasValue)
            {
                query = query.Where(c => c.ApprovedDate.Value.Date >= filterParameter.ApprovedDate.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.ArticleName))
            {
                query = query.Where(c => c.Article.Name.Contains(filterParameter.ArticleName));
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.ApprovedByName))
            {
                query = query.Where(c => c.ApprovedBy.Name.Contains(filterParameter.ApprovedByName));
            }

            if (Guid.TryParse(filterParameter.CreatedbyId, out Guid createdById) && createdById != Guid.Empty)
            {
                query = query.Where(c => c.CreatedbyId == createdById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.CreatedbyName))
            {
                query = query.Where(c => c.CreatedBy.UserName.Contains(filterParameter.CreatedbyName));
            }

            if (filterParameter.CreatedDateFrom.HasValue)
            {
                query = query.Where(c => c.CreatedDate.Date >= filterParameter.CreatedDateFrom.Value.Date);
            }

            if (filterParameter.CreatedDateTo.HasValue)
            {
                query = query.Where(c => c.CreatedDate.Date <= filterParameter.CreatedDateTo.Value.Date);
            }

            if (Guid.TryParse(filterParameter.LastUpdatedbyId, out Guid lastUpdatedById) && lastUpdatedById != Guid.Empty)
            {
                query = query.Where(c => c.LastUpdatedbyId == lastUpdatedById.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filterParameter.LastUpdatedbyName))
            {
                query = query.Where(c => c.LastUpdatedBy.UserName.Contains(filterParameter.LastUpdatedbyName));
            }

            if (filterParameter.LastUpdatedDateFrom.HasValue)
            {
                query = query.Where(c => c.LastUpdatedDate.Date >= filterParameter.LastUpdatedDateFrom.Value.Date);
            }

            if (filterParameter.LastUpdatedDateTo.HasValue)
            {
                query = query.Where(c => c.LastUpdatedDate.Date <= filterParameter.LastUpdatedDateTo.Value.Date);
            }

            // Check if a hierarchy based on ParentId is requested
            if (Guid.TryParse(filterParameter.ParentId, out Guid parentId) && parentId != Guid.Empty)
            {
                query = query.Where(c => c.ParentId == parentId.ToString());
            }


            // Apply sorting if specified
            var propertyInfo = typeof(Comment).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(c => propertyInfo.GetValue(c, null))
                    : query.OrderBy(c => propertyInfo.GetValue(c, null));
            }


            // Execute the query and return results
            return query.ToList();
        }

        // Helper method to build comment hierarchy
        //private List<Comment> BuildCommentHierarchy(IQueryable<Comment> query, string parentId)
        //{
        //    // Retrieve all comments related to the specified parent
        //    var allComments = query.Where(c => c.ParentId == parentId || c.Id == parentId)
        //                           .ToList();

        //    // Group comments by ParentId for efficient child lookups
        //    var commentsByParentId = allComments
        //        .GroupBy(c => c.ParentId)
        //        .ToDictionary(g => g.Key, g => g.ToList());

        //    // Find the root comment based on ParentId
        //    var rootComment = allComments.FirstOrDefault(c => c.Id == parentId);
        //    if (rootComment == null) return new List<Comment>(); // Return an empty list if not found

        //    // Initialize a queue to build the hierarchy
        //    var queue = new Queue<Comment>();
        //    queue.Enqueue(rootComment);

        //    var hierarchicalComments = new List<Comment>();

        //    // Process comments level-by-level
        //    while (queue.Count > 0)
        //    {
        //        var currentComment = queue.Dequeue();
        //        hierarchicalComments.Add(currentComment); // Add the current comment to the result list

        //        // Find children for the current comment
        //        if (commentsByParentId.TryGetValue(currentComment.Id, out var children))
        //        {
        //            foreach (var child in children)
        //            {
        //                queue.Enqueue(child); // Enqueue each child for further processing
        //            }
        //        }
        //    }

        //    return hierarchicalComments; // Return the list of comments in hierarchy order
        //}

        public Comment GetComment(string id)
        {
            return _context.Comment.Include(c => c.CreatedBy).Include(c => c.Parent).Include(c => c.LastUpdatedBy).FirstOrDefault(c => c.Id == id);
        }

        public List<Comment> GetCommentsByArticleIdAsync(string articleId)
        {
            return _context.Comment.Include(c => c.CreatedBy).Include(c => c.LastUpdatedBy)
                .Where(c => c.ArticleId == articleId)
                .ToList();
        }

        public bool IsCommentExists(string id)
        {
            return _context.Comment.Any(c => c.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateComment(Comment comment)
        {
            _context.Comment.Update(comment);
        }
    }
}
