using CMS_API_Application.Dto.Authorization;
using CMS_API_Core.helper.Response;

namespace CMS_API_Application.Interfaces.Servises.Authorization
{
    public interface IPermissionService
    {
        ApiResponse<object?> Create(CreatePermissionDto UserRequest);
        ApiResponse<object?> Update(UpdatePermissionDto UserRequest);
        ApiResponse<object?> DeleteById(string PermissionId);
        ApiResponse<object?> DeleteListById(List<string> PermissionsId);
        ApiResponse<object?> GetList(int skip, int take);
        ApiResponse<object?> GetById(string PermissionId);
        ApiResponse<object?> GetByName(string PermissionName, int skip, int take);
    }
}
