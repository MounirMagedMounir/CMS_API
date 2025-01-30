using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.FilterModels;
using CMS_API_Core.Interfaces.Repository.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CMS_API_Application.Dto.Authorization;
using CMS_API_Application.Interfaces.Servises.Authorization;
using CMS_API_Core.helper.Response;

namespace CMS_API.Services.Authorization
{
    public class RoleService(
        IHttpContextAccessor _httpContextAccessor
        , IRoleRepository _RoleRepository
        , IPermissionRepository _PermissionRepository
        , IRolePermissionRepository _RolePermissionRepository
        ) : IRoleService
    {
        public ApiResponse<object?> Create(CreateRoleDto UserRequest)
        {

            var NewRoleId = Guid.NewGuid().ToString();


            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            if (_RoleRepository.IsRoleNameExists(UserRequest.Name))
            {

                return new ApiResponse<object?>(
          data: null,
          status: StatusCodes.Status400BadRequest,
          message: [" Role Name Aleardy exist"]);
            }


            Role NewRole = new()
            {
                Id = NewRoleId,
                Name = UserRequest.Name,
                LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                LastUpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            _RoleRepository.AddRole(NewRole);

            if (UserRequest.Permissions != null)
            {
                var allPermissions = _PermissionRepository.GetAllPermissions();

                foreach (var permission in UserRequest.Permissions)
                {

                    if (allPermissions.Any(p => p.Name == permission.Name))
                    {
                        var newRolePermission = new RolePermission
                        {
                            Id = Guid.NewGuid().ToString(),
                            RoleId = NewRoleId,
                            PermissionId = allPermissions.FirstOrDefault(p => p.Name == permission.Name).Id

                        };

                        _RolePermissionRepository.AddRolePermission(newRolePermission);
                    }
                    else
                    {

                        return new ApiResponse<object?>(
                                   data: null,
                                   status: StatusCodes.Status404NotFound,
                                   message: [$" Permission Name : {permission.Name} dosen`t exist"]);
                    }
                }
            }

            _RoleRepository.SaveChanges();

            return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status201Created,
                    message: new List<string> { $"You have Created a role successfly" });

        }

        public ApiResponse<object?> Update(UpdateRoleDto UserRequest)
        {

            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }


            var Role = _RoleRepository.GetRoleById(UserRequest.Id);

            if (Role == null)
            {
                return new ApiResponse<object?>(
                   data: null,
                   status: StatusCodes.Status404NotFound,
                   message: [$"Role id : {UserRequest.Id} doesn't exist"]);
            }



            if (Role.Name != UserRequest.Name && _RoleRepository.IsRoleNameExists(UserRequest.Name))
            {

                return new ApiResponse<object?>(
                   data: null,
                   status: StatusCodes.Status400BadRequest,
                   message: [" Role Name Aleardy exist"]);

            }

            Role.Name = string.IsNullOrEmpty(UserRequest.Name) ? Role.Name : UserRequest.Name;
            Role.LastUpdatedDate = DateTime.Now;
            Role.LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            _RoleRepository.UpdateRole(Role);

            if (UserRequest.Permissions != null)
            {
                // Fetch all permissions once
                var allPermissions = _PermissionRepository.GetAllPermissions();

                // Fetch current role permissions
                var currentRolePermissions = _RolePermissionRepository.GetRolePermissionByRoleId(Role.Id);

                // Get permission Ids from UserRequest.Permissions
                var requestedPermissionIds = new List<string>();

                foreach (var permission in UserRequest.Permissions)
                {
                    // Check if permission exists in allPermissions by Name or Id
                    var existingPermission = allPermissions.FirstOrDefault(p => p.Name == permission.Name || p.Id == permission.Id);
                    if (existingPermission != null)
                    {
                        requestedPermissionIds.Add(existingPermission.Id);

                        // Check if this permission is already assigned to the role, if not add it
                        if (!currentRolePermissions.Any(rp => rp.PermissionId == existingPermission.Id))
                        {
                            var newRolePermission = new RolePermission
                            {
                                Id = Guid.NewGuid().ToString(),
                                RoleId = Role.Id,
                                PermissionId = existingPermission.Id
                            };
                            _RolePermissionRepository.AddRolePermission(newRolePermission);
                        }
                    }
                    else
                    {
                        // Permission doesn't exist in the system, return an alert

                        return new ApiResponse<object?>(
                                      data: null,
                                      status: StatusCodes.Status404NotFound,
                                      message: [$"Permission Name: {permission.Name} doesn't exist."]);
                    }
                }

                // Remove permissions that exist in the database but are not present in the request
                foreach (var rolePermission in currentRolePermissions)
                {
                    if (!requestedPermissionIds.Contains(rolePermission.PermissionId))
                    {
                        _RolePermissionRepository.DeleteRolePermission(rolePermission.Id);
                    }
                }
            }
            // Save the changes to the database
            _RoleRepository.SaveChanges();


            return new ApiResponse<object?>(
                  data: null,
                  status: StatusCodes.Status200OK,
                  message: new List<string> { "You have Updated the Role Successfuly" });

        }

        public ApiResponse<object?> DeleteById(string RoleId)
        {

            var role = _RoleRepository.GetRoleById(RoleId);


            if (role is null)
            {

                return new ApiResponse<object?>(
                   data: null,
                   status: StatusCodes.Status404NotFound,
                   message: ["Role id dosen`t exist"]);
            }

            _RoleRepository.DeleteRole(role.Id);
            _RoleRepository.SaveChanges();



            return new ApiResponse<object?>(
              data: null,
              status: StatusCodes.Status200OK,
              message: new List<string> { $"You have Removed Role with Id :{RoleId} successfly" });

        }

        public ApiResponse<object?> DeleteListById(List<string> RolesId)
        {
            var message = new List<string>();
            int index = 0;
            foreach (var RoleId in RolesId)
            {
                var role = _RoleRepository.GetRoleById(RoleId);


                if (role is null)
                {
                    message.Add("Role id dosen`t exist");
                    return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status404NotFound,
                    message: message);
                }

                _RoleRepository.DeleteRole(role.Id);
                message.Add($"You have Removed Role with id :{RoleId} successfly");
            }

            _RoleRepository.SaveChanges();

            return new ApiResponse<object?>(
                  data: null,
                  status: StatusCodes.Status200OK,
                  message: message);

        }

        public ApiResponse<object?> GetList(RolesFilter? filterParameter, int skip = 1, int take = 3, string? sortBy = "Name", string? sortOrder = "asc")
        {


            var validationErrors = filterParameter.Validation();

            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            var query = _RoleRepository.GetRoles(filterParameter, sortBy, sortOrder).Select(role => new GetRoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.Permission.Select(rp => new GetRolePermissionDto
                {
                    Id = rp.Id,
                    Name = rp.Name
                }).ToList(),
                CreatedbyId = role.CreatedbyId,
                CreatedByName = role.CreatedBy.UserName,
                CreatedDate = role.CreatedDate,
                LastUpdatedbyId = role.LastUpdatedbyId,
                LastUpdatedByName = role.LastUpdatedBy.UserName,
                LastUpdatedDate = role.LastUpdatedDate,

            });
            var totalRecords = query.Count();

            if (totalRecords == 0)
            {
                return new ApiResponse<object?>(
          data: null,
          status: StatusCodes.Status404NotFound,
          message: ["No Role found with the given parameters."]);
            }


            var roleList = query.Skip((skip - 1) * take).Take(take).ToList();

            var metaData = new MetaData<RolesFilter>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: filterParameter, _SortBy: sortBy, sortOrder);

            return new ApiResponse<object?>(
                    data: [roleList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Roles fetched successfully."])
            { };

        }

        public ApiResponse<object?> GetById(string RoleId)
        {

            var role = _RoleRepository.GetRoleById(RoleId);


            if (role is null)
            {
                return new ApiResponse<object?>(
                      data: null,
                      status: StatusCodes.Status404NotFound,
                      message: [$" Role Id : {RoleId} dosen`t exist"]);
            }

            var roleDto = new GetRoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.Permission.Select(rp => new GetRolePermissionDto
                {
                    Id = rp.Id,
                    Name = rp.Name
                }).ToList(),
                CreatedbyId = role.CreatedbyId,
                CreatedByName = role.CreatedBy.UserName,
                CreatedDate = role.CreatedDate,
                LastUpdatedbyId = role.LastUpdatedbyId,
                LastUpdatedByName = role.LastUpdatedBy.UserName,
                LastUpdatedDate = role.LastUpdatedDate,

            };

            return new ApiResponse<object?>(
                      data: new List<GetRoleDto> { roleDto },
                      status: StatusCodes.Status200OK,
                      message: [$"Role with Id {RoleId} fetched successfully"]);

        }
    }
}
