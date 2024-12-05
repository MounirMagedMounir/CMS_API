using CMS_API_Application.Dto.Authorization;
using CMS_API_Core.helper.Response;

namespace CMS_API_Application.Interfaces.Servises.Authorization
{
    public interface IRolePermissionService
    {
        ApiResponse<object?> AddPermissions(RolepermissionDto UserRequest);
        ApiResponse<object?> RemovePermissions(RolepermissionDto UserRequest);
    }
}
