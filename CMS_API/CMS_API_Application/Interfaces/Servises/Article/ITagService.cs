using CMS_API_Application.Dto.Article;
using CMS_API_Core.FilterModels.Article;
using CMS_API_Core.helper.Response;

namespace CMS_API_Application.Interfaces.Servises.Article
{
    public interface ITagService
    {
        ApiResponse<object?> Create(CreateTagDto UserRequest);
        ApiResponse<object?> Update(UpdateTagDto UserRequest);
        ApiResponse<object?> Delete(string Id);
        ApiResponse<object?> DeleteList(List<string> ids);
        ApiResponse<object?> GetList(TagsFilter filterParameter, int skip, int take, string sortBy, string sortOrder);
        ApiResponse<object?> GetById(string id);
        ApiResponse<object?> GetAll(int skip, int take);
    }
}
