using CMS_API_Application.Dto.Authorization;
using CMS_API_Application.Interfaces.Servises.Authorization;
using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.helper.Response;
using CMS_API_Core.Interfaces.Repository.Authorization;
using Microsoft.AspNetCore.Http;

namespace CMS_API.Services.Authorization
{
    public class RolePermissionService(IRoleRepository _RoleRepository
        , IPermissionRepository _PermissionRepository
        , IRolePermissionRepository _RolePermissionRepository

        ) : IRolePermissionService
    {
        public ApiResponse<object?> AddPermissions(RolepermissionDto UserRequest)
        {
            if (UserRequest is null)
            {
                return new ApiResponse<object?>(
                      data: null,
                      status: StatusCodes.Status400BadRequest,
                      message: [" Role(Name/Id) and Permission(Name/Id) is Requered "]);
            }
            var role = _RoleRepository.GetRoleByIdOrName(UserRequest.RoleId, UserRequest.RoleName);
            if (role is null)
            {

                return new ApiResponse<object?>(
                     data: null,
                     status: StatusCodes.Status404NotFound,
                     message: [" No Role (Name/Id) exist"]);
            }

            var message = new List<string>();
            int index = 0;
            if (UserRequest.PermissionsName is not null)
                foreach (var permission in UserRequest.PermissionsName)
                {

                    if (permission is not null && _PermissionRepository.GetPermissionByName(permission) is null)
                    {

                        return new ApiResponse<object?>(
                            data: null,
                            status: StatusCodes.Status404NotFound,
                            message: ["Permission Name dose not exist"]);
                    }
                    else if (permission is not null && _RolePermissionRepository.GetRolePermissionByRoleId(role.Id).Any(rp => rp.Permission.Name == permission))
                    {
                        return new ApiResponse<object?>(
                           data: null,
                           status: StatusCodes.Status400BadRequest,
                           message: [" Permission already exist"]);
                    }
                    else if (permission is not null)
                    {

                        _RolePermissionRepository.AddRolePermission(new RolePermission
                        {
                            Id = Guid.NewGuid().ToString(),
                            RoleId = role.Id,
                            PermissionId = _PermissionRepository.GetPermissionByName(permission).Id
                        });
                        message.Add($"You have Added Permission {permission} to a Role {role.Name} successfly");
                    }

                }


            if (UserRequest.PermissionsId is not null)
                foreach (var permission in UserRequest.PermissionsId)
                {
                    if (permission is not null && _PermissionRepository.GetPermissionById(permission) is null)
                    {

                        return new ApiResponse<object?>(
                               data: null,
                               status: StatusCodes.Status404NotFound,
                               message: [" No Permission Id exist"]);
                    }
                    else if (permission is not null && _RolePermissionRepository.GetRolePermissionByRoleId(role.Id).Any(rp => rp.Permission.Id == permission))
                    {

                        return new ApiResponse<object?>(
                                    data: null,
                                    status: StatusCodes.Status400BadRequest,
                                    message: [" Permission already exist"]);
                    }
                    else if (permission is not null)
                    {
                        _RolePermissionRepository.AddRolePermission(new RolePermission
                        {
                            Id = Guid.NewGuid().ToString(),
                            RoleId = role.Id,
                            PermissionId = permission
                        });
                        message.Add($"You have Added Permission {permission} to the Role {role.Name} successfly");
                    }
                }



            _RolePermissionRepository.SaveChanges();


            return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status200OK,
                    message: message);



        }

        public ApiResponse<object?> RemovePermissions(RolepermissionDto UserRequest)
        {
            if (UserRequest is null)
            {
                return new ApiResponse<object?>(
                              data: null,
                              status: StatusCodes.Status400BadRequest,
                              message: [" Role(Name/Id) and Permission(Name/Id) is Requered "]);
            }
            var role = _RoleRepository.GetRoleByIdOrName(UserRequest.RoleId, UserRequest.RoleName);

            if (role is null)
            {

                return new ApiResponse<object?>(
                              data: null,
                              status: StatusCodes.Status404NotFound,
                              message: [" No Role (Name/Id) exist"]);
            }

            var message = new List<string>();
            int index = 0;
            if (UserRequest.PermissionsName is not null)
                foreach (var UserPermission in UserRequest.PermissionsName)
                {
                    var permission = _PermissionRepository.GetPermissionByName(UserPermission);
                    if (UserPermission is not null && permission is null)
                    {

                        return new ApiResponse<object?>(
                                   data: null,
                                   status: StatusCodes.Status400BadRequest,
                                   message: [" No Permission Name exist"]);
                    }
                    else if (UserPermission is not null && !_RolePermissionRepository.GetRolePermissionByRoleId(role.Id).Any(rp => rp.Permission.Name == UserPermission))
                    {

                        return new ApiResponse<object?>(
                                   data: null,
                                   status: StatusCodes.Status400BadRequest,
                                   message: [" Permission Name already dosen`t exist in the Role"]);
                    }
                    else if (UserPermission is not null)
                    {

                        _RolePermissionRepository.DeleteRolePermission(_RolePermissionRepository.GetRolePermissionByRoleIdANDPermissionId(role.Id, permission.Id).Id);

                        message.Add($"You have Removed Permission {UserPermission} to a Role {role.Name} successfly");
                    }

                }


            if (UserRequest.PermissionsId is not null)
                foreach (var permission in UserRequest.PermissionsId)
                {
                    if (permission is not null && _PermissionRepository.GetPermissionById(permission) is null)
                    {

                        return new ApiResponse<object?>(
                                data: null,
                                status: StatusCodes.Status404NotFound,
                                message: [" No Permission Id exist"]);
                    }
                    else if (permission is not null && !_RolePermissionRepository.GetRolePermissionByRoleId(role.Id).Any(rp => rp.Permission.Id == permission))
                    {

                        return new ApiResponse<object?>(
                               data: null,
                               status: StatusCodes.Status404NotFound,
                               message: ["Permission Id already dosen`t exist in the Role"]);
                    }
                    else if (permission is not null)
                    {
                        _RolePermissionRepository.DeleteRolePermission(_RolePermissionRepository.GetRolePermissionByRoleIdANDPermissionId(role.Id, permission).Id);
                        message.Add($"You have Removed Permission {permission} to a Role {role.Name} successfly");
                    }
                }



            _RolePermissionRepository.SaveChanges();


            return new ApiResponse<object?>(
                  data: null,
                  status: StatusCodes.Status200OK,
                  message: message);
        }
    }

}