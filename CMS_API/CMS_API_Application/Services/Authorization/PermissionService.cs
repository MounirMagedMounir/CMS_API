using CMS_API_Core.Interfaces.Repository.Authorization;
using CMS_API_Core.DomainModels.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CMS_API_Application.Dto.Authorization;
using CMS_API_Application.Interfaces.Servises.Authorization;
using CMS_API_Core.helper.Response;

namespace CMS_API.Services.Authorization
{
    public class PermissionService(
         IHttpContextAccessor _httpContextAccessor
        , IPermissionRepository _PermissionRepository
        ) : IPermissionService
    {
        public ApiResponse<object?> Create(CreatePermissionDto UserRequest)
        {


            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            var NewPermissionId = Guid.NewGuid().ToString();


            if (_PermissionRepository.IsPermissionExists(UserRequest.Name))
            {
                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status400BadRequest,
                        message: [" Permission Name Aleardy exist"]);
            }


            Permission NewPermission = new()
            {
                Id = NewPermissionId,
                Name = UserRequest.Name,
                LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                LastUpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            _PermissionRepository.AddPermission(NewPermission);
            _PermissionRepository.SaveChanges();


            return new ApiResponse<object?>(
                      data: null,
                      status: StatusCodes.Status201Created,
                      message: new List<string> { $"You have Created a new Permission successfly" });

        }

        public ApiResponse<object?> Update(UpdatePermissionDto UserRequest)
        {


            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }
            var permission = _PermissionRepository.GetPermissionById(UserRequest.Id);

            if (permission is null)
            {
                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status404NotFound,
                        message: [" permission Id dosen`t exist"]);
            }

            if (_PermissionRepository.IsPermissionExists(UserRequest.Name))
            {
                return new ApiResponse<object?>(
                         data: null,
                         status: StatusCodes.Status400BadRequest,
                         message: [" Permission Name Aleardy exist"]);
            }



            permission.Name = UserRequest.Name;
            permission.LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            permission.LastUpdatedDate = DateTime.Now;


            _PermissionRepository.UpdatePermission(permission);
            _PermissionRepository.SaveChanges();

            return new ApiResponse<object?>(
                      data: null,
                      status: StatusCodes.Status200OK,
                      message: new List<string> { "You have Updated the permission Successfuly" });

        }

        public ApiResponse<object?> DeleteById(string PermissionId)
        {


            var permission = _PermissionRepository.GetPermissionById(PermissionId);


            if (permission is null)
            {

                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status404NotFound,
                        message: ["permission id dosen`t exist"]);
            }

            _PermissionRepository.DeletePermission(permission.Id);
            _PermissionRepository.SaveChanges();

            return new ApiResponse<object?>(
               data: null,
               status: StatusCodes.Status200OK,
               message: new List<string> { $"You have removed permission with Id :{PermissionId} successfly" });

        }

        public ApiResponse<object?> DeleteListById(List<string> PermissionsId)
        {
            var message = new List<string>();
            int index = 0;
            foreach (var PermissionId in PermissionsId)
            {
                var permission = _PermissionRepository.GetPermissionById(PermissionId);


                if (permission is null)
                {
                    return new ApiResponse<object?>(
                             data: null,
                             status: StatusCodes.Status404NotFound,
                             message: ["permission id dosen`t exist"]);
                }

                _PermissionRepository.DeletePermission(permission.Id);
                message.Add($"You have removed permission with Id :{PermissionId} successfly");
            }
            _PermissionRepository.SaveChanges();



            return new ApiResponse<object?>(
                data: null,
                status: StatusCodes.Status200OK,
                message: message);

        }

        public ApiResponse<object?> GetList(int skip, int take)
        {

            var Permissions = _PermissionRepository.GetPermissions().Select(permission => new GetPermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                CreatedDate = permission.CreatedDate,
                CreatedbyId = permission.CreatedbyId,
                LastUpdatedbyId = permission.LastUpdatedbyId,
                LastUpdatedDate = permission.LastUpdatedDate,
                CreatedByName = permission.CreatedBy.Name,
                LastUpdatedByName = permission.LastUpdatedBy.Name

            });

            var totalRecords = Permissions.Count();

            if (totalRecords == 0)
            {
                return new ApiResponse<object?>(
          data: null,
          status: StatusCodes.Status404NotFound,
          message: ["No Permissions found with the given parameters."]);
            }

            var permissionList = Permissions.Skip((skip - 1) * take).Take(take).ToList();
            var metaData = new MetaData<object>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: null);

            return new ApiResponse<object?>(
                    data: [permissionList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Users fetched successfully."])
            { };

        }

        public ApiResponse<object?> GetById(string PermissionId)
        {

            var permission = _PermissionRepository.GetPermissionById(PermissionId);

            if (permission is null)
            {
                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status404NotFound,
                        message: [$" Permission id {PermissionId}dosen`t exist"]);
            }


            var PermissionDto = new GetPermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                CreatedDate = permission.CreatedDate,
                CreatedbyId = permission.CreatedbyId,
                LastUpdatedbyId = permission.LastUpdatedbyId,
                LastUpdatedDate = permission.LastUpdatedDate,
                CreatedByName = permission.CreatedBy.Name,
                LastUpdatedByName = permission.LastUpdatedBy.Name

            };

            return new ApiResponse<object?>(
            data: [PermissionDto],
            status: StatusCodes.Status200OK,
            message: ["Permission fetched Successfully"]);

        }

        public ApiResponse<object?> GetByName(string PermissionName, int skip, int take)
        {

            var permission = _PermissionRepository.GetPermissionsByName(PermissionName).Select(permission => new GetPermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                CreatedDate = permission.CreatedDate,
                CreatedbyId = permission.CreatedbyId,
                LastUpdatedbyId = permission.LastUpdatedbyId,
                LastUpdatedDate = permission.LastUpdatedDate,
                CreatedByName = permission.CreatedBy.Name,
                LastUpdatedByName = permission.LastUpdatedBy.Name

            });
            var totalRecords = permission.Count();

            if (totalRecords == 0)
            {
                return new ApiResponse<object?>(
          data: null,
          status: StatusCodes.Status404NotFound,
          message: [
              $"Permissio Name :{PermissionName} dosen`t exist"]);
            }

            var permissionList = permission.Skip((skip - 1) * take).Take(take).ToList();

            var metaData = new MetaData<string>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: PermissionName);

            return new ApiResponse<object?>(
                    data: [permissionList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Users fetched successfully."])
            { };

        }
    }
}
