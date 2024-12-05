using CMS_API_Application.Dto.Authorization;
using CMS_API_Core.FilterModels;
using CMS_API_Core.helper.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Application.Interfaces.Servises.Authorization
{
    public interface IRoleService
    {
        ApiResponse<object?> Create(CreateRoleDto UserRequest);
        ApiResponse<object?> Update(UpdateRoleDto UserRequest);
        ApiResponse<object?> DeleteById(string RoleId);
        ApiResponse<object?> DeleteListById(List<string> RolesId);
        ApiResponse<object?> GetList(RolesFilter? filterParameter, int skip = 1, int take = 3, string? sortBy = "Name", string? sortOrder = "asc");
        ApiResponse<object?> GetById(string RoleId);

    }
}
