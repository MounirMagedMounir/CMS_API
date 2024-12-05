using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.FilterModels.Article;

namespace CMS_API_Core.Interfaces.Repository.Article
{
    public interface ICommentRepository
    {
        List<Comment> GetCommentsByArticleIdAsync(string articleId);
        IEnumerable<Comment> GetComments(CommentsFilter filterParameter, string sortBy, string sortOrder);
        Comment GetComment(string id);
        void AddComment(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(string id);
        bool IsCommentExists(string id);
        void SaveChanges();
    }
}
