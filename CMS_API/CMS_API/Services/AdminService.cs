using CMS_API.Dto.Admin;
using CMS_API.Dto.Filters;
using CMS_API_Core.DomainModels;
using CMS_API_Core.FilterModels;
using CMS_API_Core.helper.Utils;
using CMS_API_Core.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Security.Claims;


namespace CMS_API.Services
{
    public class AdminService(IConfiguration configuration
        , IHttpContextAccessor _httpContextAccessor
         , IUserRepository _userRepository
          , IRoleRepository _RoleRepository
        , IPermissionRepository _PermissionRepository
        , IRolePermissionRepository _RolePermissionRepository
        , IMemoryCache _cache
        ) : ControllerBase
    {

        readonly EmailSender emailSender = new(configuration);
        readonly Authentication authentication = new(configuration);


        public ActionResult CreateUser(UserDto UserRequest)
        {
            var Respons = new Dictionary<string, object>();

            var NewUserId = Guid.NewGuid().ToString();

            var Role = _RoleRepository.GetRoleByName(UserRequest.Role);

            if (Role is null)
            {
                Respons["Alert"] = " Role dosen`t exist";
                return BadRequest(Respons.ToList());
            }

            User NewUser = new()
            {
                Id = NewUserId,
                Name = UserRequest.Name,
                Email = UserRequest.Email,
                UserName = UserRequest.UserName,
                Phone = UserRequest.Phone,
                RoleId = Role.Id,
                IsActive = UserRequest.IsActive,
                LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                LastUpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            var Password = Guid.NewGuid().ToString().Substring(0, 10).Insert(1, "@mM1");

            NewUser.Password = Password;

            if (NewUser.Validation().Count > 0)
            {
                return BadRequest(NewUser.Validation().ToList());

            }

            if (_userRepository.IsEmailExists(UserRequest.Email))
            {
                Respons["Alert"] = " Email Aleardy exist";
                return BadRequest(Respons.ToList());
            }


            _userRepository.AddUser(NewUser);
            _userRepository.SaveChangesAsync();

            return emailSender.EmailMessage(NewUser.Email, "registration Email", $"Hi {NewUser.Name} " +
                  $"  {NewUser.Email}   " +
                  "                                                                " + $"  {NewUser.Password}   " +
                  " is your New Password and Email  .");

        }

        public ActionResult UpdateUser(UserDto UserRequest)
        {
            var Respons = new Dictionary<string, object>();

            var NewUserId = Guid.NewGuid();
            if (UserRequest.Id is null)
            {
                Respons["Alert"] = " User Id is required";
                return BadRequest(Respons.ToList());
            }
            var user = _userRepository.GetUserById(UserRequest.Id);
            if (user is null)
            {
                Respons["Alert"] = " User Id dosen`t exist";
                return BadRequest(Respons.ToList());
            }

            if (user.Email != UserRequest.Email)
                if (_userRepository.IsEmailExists(UserRequest.Email))
                {
                    Respons["Alert"] = " Email Aleardy exist";
                    return BadRequest(Respons.ToList());
                }



            var Role = _RoleRepository.GetRoleByName(UserRequest.Role);

            if (Role is null)
            {
                Respons["Alert"] = " Role dosen`t exist";
                return BadRequest(Respons.ToList());
            }

            user.Name = string.IsNullOrEmpty(UserRequest.Name) ? user.Name : UserRequest.Name;

            user.Email = string.IsNullOrEmpty(UserRequest.Email) ? user.Email : UserRequest.Email;

            user.UserName = string.IsNullOrEmpty(UserRequest.UserName) ? user.UserName : UserRequest.UserName;

            user.Phone = string.IsNullOrEmpty(UserRequest.Phone) ? user.Phone : UserRequest.Phone;

            user.RoleId = string.IsNullOrEmpty(Role.Id) ? user.RoleId : Role.Id;

            user.IsActive = UserRequest.IsActive;
            user.Password = string.IsNullOrEmpty(UserRequest.Password) ? user.Password : UserRequest.Password;
            user.ProfileImage = string.IsNullOrEmpty(UserRequest.ProfileImage) ? user.ProfileImage : UserRequest.ProfileImage;
            user.LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            user.LastUpdatedDate = DateTime.Now;

            if (user.Validation().Count > 0)
            {

                return BadRequest(user.Validation().ToList());

            }

            _userRepository.UpdateUser(user);
            _userRepository.SaveChangesAsync();

            Respons["Success"] = "You have Updated the user Successfuly";
            return Ok(Respons.ToList());

        }

        public ActionResult DeleteUser(string UserId)
        {
            var Respons = new Dictionary<string, object>();

            var user = _userRepository.GetUserById(UserId);


            if (user is null)
            {
                Respons["Alert"] = " user id dosen`t exist";
                return BadRequest(Respons.ToList());
            }

            _userRepository.DeleteUser(user.Id);
            _userRepository.SaveChangesAsync();


            Respons["Success"] = $"You have removed user with id :{UserId} successfly";
            return Ok(Respons.ToList());

        }

        public ActionResult DeleteUsers(List<string> UsersId)
        {
            var Respons = new Dictionary<string, object>();
            foreach (var UserId in UsersId)
            {
                var user = _userRepository.GetUserById(UserId);


                if (user is null)
                {
                    Respons["Alert"] = $" user id{UserId} dosen`t exist";
                }

                _userRepository.DeleteUser(user.Id);
                Respons["Success"] = $"You have removed user with id :{UserId} successfly";
            }

            if (Respons.ContainsKey("Alert"))
                return BadRequest(Respons.ToList());


            _userRepository.SaveChangesAsync();

            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> GetUsers(UsersFilter parameter, int skip, int take, string? sortBy = "Name", string? sortOrder = "asc")
        {
            var response = new Dictionary<string, object>();


            var validationErrors = parameter.Validation();
            if (validationErrors.Count > 0)
            {
                return BadRequest(validationErrors.ToList());
            }

            var users = _userRepository.GetUsers(parameter, sortBy, sortOrder).Select(user => new FilterUserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                ProfileImage = user.ProfileImage,
                IsActive = user.IsActive,
                Role = user.Role.Name.ToString(),
                CreatedDate = user.CreatedDate,
                LastUpdatedDate = user.LastUpdatedDate,
                CreatedbyId = user.CreatedbyId,
                LastUpdatedbyId = user.LastUpdatedbyId,
            });
            var totalRecords = users.Count();

            if (totalRecords == 0)
            {
                response["Alert"] = "No users found with the given parameters.";
                return NotFound(response.ToList());
            }

            response["Success"] = users.Skip((skip - 1) * take).Take(take);
            response["Count"] = totalRecords;
            response["Skip"] = skip;
            response["Take"] = take;

            return Ok(response.ToList());
        }

        public async Task<ActionResult> GetUser(string parameter, int skiped, int take)
        {
            var Respons = new Dictionary<string, object>();

            var user = _userRepository.GetUsersByParameter(parameter);
            if (!user.Any())
            {
                Respons["Alert"] = $"user parameter: {parameter} dosen`t exist";
                return BadRequest(Respons.ToList());
            }


            Respons["Success"] = user.Skip((skiped - 1) * take).Take(take);
            Respons["Count"] = user.Count();
            Respons["Skip"] = skiped;
            Respons["Take"] = take;
            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> GetUserById(string UserId)
        {
            var Respons = new Dictionary<string, object>();


            var user = _userRepository.GetUserById(UserId);

            if (user is null)
            {
                Respons["Alert"] = "user id dosen`t exist";
                return BadRequest(Respons.ToList());
            }


            var userDto = new
            {
                user.Id,
                user.Name,
                user.UserName,
                user.Email,
                user.IsActive,
                user.Phone,
                Role = user.Role.Name,
                user.Password,
                user.CreatedDate,
                user.LastUpdatedDate,
                user.CreatedbyId,
                user.LastUpdatedbyId
            };

            Respons["Success"] = userDto;
            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> GetUserByName(string UserName, int skiped, int take)
        {
            var Respons = new Dictionary<string, object>();



            var user = _userRepository.GetUsersByName(UserName);

            if (!user.Any())
            {
                Respons["Alert"] = $"user Name :{UserName} dosen`t exist";
                return BadRequest(Respons.ToList());
            }


            Respons["Success"] = user.Skip((skiped - 1) * take).Take(take).ToList();
            Respons["Count"] = user.Count();
            Respons["Skip"] = skiped;
            Respons["Take"] = take;
            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> GetUserByRole(string UserRole, int skiped, int take)
        {

            var Respons = new Dictionary<string, object>();



            var users = _userRepository.GetUsersByRole(UserRole);

            if (users is null)
            {
                Respons["Alert"] = $" Role Name :{UserRole} dosen`t exist";
                return BadRequest(Respons.ToList());
            }

            if (!users.Any())
            {
                Respons["Alert"] = $" there is no Users with the Role Name :{UserRole} ";
                return BadRequest(Respons.ToList());
            }

            Respons["Success"] = users.Skip((skiped - 1) * take).Take(take).ToList();
            Respons["Count"] = users.Count();
            Respons["Skip"] = skiped;
            Respons["Take"] = take;
            return Ok(Respons.ToList());

        }


        ////////////////    Rolse API
        public ActionResult CreateRole(RoleDto UserRequest)
        {
            var Respons = new Dictionary<string, object>();

            var NewRoleId = Guid.NewGuid().ToString();

            if (UserRequest.Name != null && UserRequest.Id != null)
            {
                Respons["Alert"] = " Role Name is requerd";
                return BadRequest(Respons.ToList());
            }

            if (_RoleRepository.IsRoleExists(UserRequest.Name))
            {
                Respons["Alert"] = " Role Name Aleardy exist";
                return BadRequest(Respons.ToList());
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

                    if (allPermissions.Any(p => p.Name == permission.Name || p.Id == permission.Id))
                    {
                        var newRolePermission = new RolePermission
                        {
                            Id = Guid.NewGuid().ToString(),
                            RoleId = NewRoleId,
                            PermissionId = !allPermissions.Any(p => p.Name == permission.Id) ? allPermissions.FirstOrDefault(p => p.Name == permission.Name).Id : permission.Id

                        };

                        _RolePermissionRepository.AddRolePermission(newRolePermission);
                    }
                    else
                    {
                        Respons["Alert"] = $" Permission Name : {permission.Name} dosen`t exist";
                        return BadRequest(Respons.ToList());
                    }
                }
            }

            _RoleRepository.SaveChangesAsync();

            Respons["Success"] = $"You have Created a role successfly";
            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> UpdateRole(RoleDto UserRequest)
        {
            var Respons = new Dictionary<string, object>();

            var Role = _RoleRepository.GetRoleByIdOrName(UserRequest.Id, UserRequest.Name);

            if (Role == null)
            {
                Respons["Alert"] = $"Role id : {UserRequest.Id} doesn't exist";
                return NotFound(Respons.ToList());
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
                        Respons["Alert"] = $"Permission Name: {permission.Name} doesn't exist.";
                        return BadRequest(Respons.ToList());
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
            await _RoleRepository.SaveChangesAsync();

            Respons["Success"] = "You have Updated the Role Successfuly";
            return Ok(Respons.ToList());

        }

        public ActionResult DeleteRoleById(string RoleId)
        {
            var Respons = new Dictionary<string, object>();

            var role = _RoleRepository.GetRoleById(RoleId);


            if (role is null)
            {
                Respons["Alert"] = "Role id dosen`t exist";
                return BadRequest(Respons.ToList());
            }

            _RoleRepository.DeleteRole(role.Id);
            _RoleRepository.SaveChangesAsync();


            Respons["Success"] = $"You have removed role with id :{RoleId} successfly";
            return Ok(Respons.ToList());

        }

        public ActionResult DeleteRolesById(List<string> RolesId)
        {
            var Respons = new Dictionary<string, object>();
            foreach (var RoleId in RolesId)
            {
                var role = _RoleRepository.GetRoleById(RoleId);


                if (role is null)
                {
                    Respons["Alert"] = "Role id dosen`t exist";

                }

                _RoleRepository.DeleteRole(role.Id);
                Respons["Success"] = $"You have removed role with id :{RoleId} successfly";
            }

            if (Respons.ContainsKey("Alert"))
                return BadRequest(Respons.ToList());


            _RoleRepository.SaveChangesAsync();

            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> GetRoles(RolesFilter? parameter, int skip = 1, int take = 3, string? sortBy = "Name", string? sortOrder = "asc")
        {
            var Respons = new Dictionary<string, object>();

            var query = _RoleRepository.GetRoles(parameter, sortBy, sortOrder).Select(role => new
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.RolePermission.Select(rp => new Permission
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name
                }).ToList(),
                CreatedDate = role.CreatedDate,
                LastUpdatedDate = role.LastUpdatedDate,
                CreatedbyId = role.CreatedbyId,
                LastUpdatedbyId = role.LastUpdatedbyId,
            });
            var totalRecords = query.Count();

            if (totalRecords == 0)
            {
                Respons["Alert"] = "No Role found with the given parameters.";
                return NotFound(Respons.ToList());
            }


            Respons["Success"] = query.Skip((skip - 1) * take).Take(take);
            Respons["Count"] = totalRecords;
            Respons["Skip"] = skip;
            Respons["Take"] = take;

            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> GetRoleById(string RoleId)
        {
            var Respons = new Dictionary<string, object>();

            var role = _RoleRepository.GetRoleById(RoleId);

            Console.WriteLine("role  " + role.Name);
            if (role is null)
            {
                Respons["Alert"] = $" role id : {RoleId} dosen`t exist";
                return BadRequest(Respons.ToList());
            }

            var roleDto = new
            {
                role.Id,
                role.Name,
                Permissions = role.RolePermission
           .Select(rp => new PermissionDto
           {
               Id = rp.Permission.Id,
               Name = rp.Permission.Name
           })
           .ToList(),
                role.CreatedDate,
                role.CreatedbyId,
                role.LastUpdatedbyId,
                role.LastUpdatedDate
            };
            Console.WriteLine("roleDto  " + roleDto.Name);
            Respons["Success"] = roleDto;
            return Ok(Respons.ToList());

        }


        public async Task<ActionResult> GetRoleByName(string RoleName, int skiped, int take)
        {
            var Respons = new Dictionary<string, object>();



            var role = _RoleRepository.GetRolesByName(RoleName).Select(role => new
            {
                role.Id,
                role.Name,
                Permissions = role.RolePermission.Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name
                }).ToList(),
                role.CreatedDate,
                role.CreatedbyId,
                role.LastUpdatedbyId,
                role.LastUpdatedDate,
            });

            if (!role.Any())
            {
                Respons["Alert"] = $"Role Name :{RoleName} dosen`t exist";
                return BadRequest(Respons.ToList());
            }


            Respons["Success"] = role.Skip((skiped - 1) * take).Take(take).ToList();
            Respons["Count"] = role.Count();
            Respons["Skip"] = skiped;
            Respons["Take"] = take;
            return Ok(Respons.ToList());

        }


        //////////////////Permission API

        public ActionResult CreatePermission(PermissionDto UserRequest)
        {
            var Respons = new Dictionary<string, object>();

            var NewPermissionId = Guid.NewGuid().ToString();



            if (_PermissionRepository.IsPermissionExists(UserRequest.Name))
            {
                Respons["Alert"] = " Permission Name Aleardy exist";
                return BadRequest(Respons.ToList());
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
            _PermissionRepository.SaveChangesAsync();

            Respons["Success"] = $"You have Created a new Permission successfly";
            return Ok(Respons.ToList());

        }

        public ActionResult UpdatePermission(PermissionDto UserRequest)
        {
            var Respons = new Dictionary<string, object>();

            var NewpermissionId = Guid.NewGuid().ToString();
            var permission = _PermissionRepository.GetPermissionById(UserRequest.Id);

            if (permission is null)
            {
                Respons["Alert"] = " permission Id dosen`t exist";
                return BadRequest(Respons.ToList());
            }

            permission.Name = string.IsNullOrEmpty(UserRequest.Name) ? permission.Name : UserRequest.Name;
            permission.LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            permission.LastUpdatedDate = DateTime.Now;


            _PermissionRepository.UpdatePermission(permission);
            _PermissionRepository.SaveChangesAsync();

            Respons["Success"] = "You have Updated the permission Successfuly";
            return Ok(Respons.ToList());

        }

        public ActionResult DeletePermissionById(string PermissionId)
        {
            var Respons = new Dictionary<string, object>();

            var permission = _PermissionRepository.GetPermissionById(PermissionId);


            if (permission is null)
            {
                Respons["Alert"] = "permission id dosen`t exist";
                return BadRequest(Respons.ToList());
            }

            _PermissionRepository.DeletePermission(permission.Id);
            _PermissionRepository.SaveChangesAsync();


            Respons["Success"] = $"You have removed permission with id :{PermissionId} successfly";
            return Ok(Respons.ToList());

        }

        public ActionResult DeletePermissionsById(List<string> PermissionsId)
        {
            var Respons = new Dictionary<string, object>();
            foreach (var PermissionId in PermissionsId)
            {
                var permission = _PermissionRepository.GetPermissionById(PermissionId);


                if (permission is null)
                {
                    Respons["Alert"] = "permission id dosen`t exist";
                    return BadRequest(Respons.ToList());
                }

                _PermissionRepository.DeletePermission(permission.Id);
                Respons["Success"] = $"You have removed permission with id :{PermissionId} successfly";
            }
            _PermissionRepository.SaveChangesAsync();



            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> GetPermissions(int skiped, int take)
        {
            var Respons = new Dictionary<string, object>();



            var Permissions = _PermissionRepository.GetPermissions().Select(permission => new
            {
                permission.Id,
                permission.Name,
                permission.RolePermission
            });


            Respons["Success"] = Permissions.Skip((skiped - 1) * take).Take(take);
            Respons["Count"] = Permissions.Count();
            Respons["Skip"] = skiped;
            Respons["Take"] = take;
            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> GetPermissionById(string PermissionId)
        {
            var Respons = new Dictionary<string, object>();


            var permission = _PermissionRepository.GetPermissionById(PermissionId);

            if (permission is null)
            {
                Respons["Alert"] = $" Permission id {PermissionId}dosen`t exist";
                return BadRequest(Respons.ToList());
            }


            Respons["Success"] = permission;
            return Ok(Respons.ToList());

        }

        public async Task<ActionResult> GetPermissionByName(string PermissionName, int skiped, int take)
        {
            var Respons = new Dictionary<string, object>();


            var permission = _PermissionRepository.GetPermissionsByName(PermissionName);

            if (!permission.Any())
            {
                Respons["Alert"] = $"Permissio Name :{PermissionName} dosen`t exist";
                return BadRequest(Respons.ToList());
            }


            Respons["Success"] = permission.Skip((skiped - 1) * take).Take(take).ToList();
            Respons["Count"] = permission.Count();
            Respons["Skip"] = skiped;
            Respons["Take"] = take;
            return Ok(Respons.ToList());

        }
    }
}
