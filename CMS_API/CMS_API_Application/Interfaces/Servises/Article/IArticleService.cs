using CMS_API_Application.Dto.Article;
using CMS_API_Core.FilterModels.Article;
using CMS_API_Core.helper.Response;

namespace CMS_API_Application.Interfaces.Servises.Article
{
    public interface IArticleService
    {
        ApiResponse<object?> Create(CreateArticleDto UserRequest);
        ApiResponse<object?> Update(UpdateArticleDto UserRequest);
        ApiResponse<object?> DeleteById(string ArticleId);
        ApiResponse<object?> GetList(ArticleFilter filterParameter, int skip, int take, string sortBy = "Name", string sortOrder = "asc");
        ApiResponse<object?> GetById(string articleId);
        ApiResponse<object?> GetListByUserId(string UserId, int skip, int take, string sortBy = "Name", string sortOrder = "asc");
        ApiResponse<object?> GetCurrentUser(int skip, int take, string sortBy = "Name", string sortOrder = "asc");
        ApiResponse<object?> UpdateViewCount(string ArticleId);


    }
}
