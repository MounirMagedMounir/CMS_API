using CMS_API_Application.Dto;
using CMS_API_Application.Dto.Admin;
using CMS_API_Core.FilterModels;
using CMS_API_Core.helper.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Application.Interfaces.Servises
{
    public interface IUserService
    {
        ApiResponse<object?> Create(CreateUserDto UserRequest);
        ApiResponse<object?> Update(UpdateUserDto UserRequest);
        ApiResponse<object?> DeletePermanent(string UserId);
        ApiResponse<object?> Delete(string UserId);
        public ApiResponse<object?> GetList(UsersFilter? filterParameter, int skip, int take, string? sortBy = "Name", string? sortOrder = "asc");
        public ApiResponse<object?> Search(string parameter, int skip, int take);
        public ApiResponse<object?> GetCurrent();
        public ApiResponse<object?> GetById(string UserId);


    }
}
