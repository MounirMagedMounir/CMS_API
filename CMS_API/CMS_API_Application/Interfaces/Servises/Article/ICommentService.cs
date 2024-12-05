using CMS_API_Application.Dto.Article;
using CMS_API_Core.FilterModels.Article;
using CMS_API_Core.helper.Response;

namespace CMS_API_Application.Interfaces.Servises.Article
{
    public interface ICommentService
    {
        ApiResponse<object?> Add(AddCommentDto UserRequest);
        ApiResponse<object?> UpdateStat(UpdateCommentDto UserRequest);
        ApiResponse<object?> Delete(string id);
        ApiResponse<object?> GetByArticleId(string articleId);
        ApiResponse<object?> GetById(string Id);
        ApiResponse<object?> GetList(CommentsFilter filterParameter, int skip, int take, string sortBy = "Name", string sortOrder = "asc");
        List<GetArticleCommentDto> BuildCommentHierarchy(string articleId);

    }
}
