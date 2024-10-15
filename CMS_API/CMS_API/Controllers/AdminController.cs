using CMS_API.Dto.Admin;
using CMS_API.Services;
using CMS_API_Core.FilterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Roles = "12483a8a-ee39-4812-b02e-d9575dde6c65")]
    public class AdminController(AdminService adminService
        ) : ControllerBase
    {
        // Users API
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Admin");
        }

        [HttpPost("CreateUser")]
        public ActionResult CreateUser(UserDto UserRequest)
        {

            return adminService.CreateUser(UserRequest);

        }
        [HttpPut("UpdateUser")]
        public ActionResult UpdateUser(UserDto UserRequest)
        {

            return adminService.UpdateUser(UserRequest);

        }

        [HttpDelete("DeleteUser")]
        public ActionResult DeleteUser(string UserId)
        {

            return adminService.DeleteUser(UserId);

        }

        [HttpDelete("DeleteUsers")]
        public ActionResult DeleteUsers([FromBody] List<string> UserId)
        {
            return adminService.DeleteUsers(UserId);
        }

        [HttpPost("GetUsers")]
        public Task<ActionResult> GetUsers(UsersFilter? parameter, int skiped, int take, string? sortBy, string? sortOrder)
        {

            return adminService.GetUsers(parameter, skiped, take, sortBy, sortOrder);

        }

        [HttpGet("GetUser")]
        public Task<ActionResult> GetUser(string parameter, int skiped, int take)
        {

            return adminService.GetUser(parameter, skiped, take);

        }

        [HttpGet("GetUserById")]
        public Task<ActionResult> GetUserById(string UserId)
        {

            return adminService.GetUserById(UserId);

        }

        [HttpGet("GetUserByName")]
        public Task<ActionResult> GetUserByName(string UserName, int skiped, int take)
        {

            return adminService.GetUserByName(UserName, skiped, take);

        }

        [HttpGet("GetUserByRole")]
        public Task<ActionResult> GetUserByRole(string UserRole, int skiped, int take)
        {

            return adminService.GetUserByRole(UserRole, skiped, take);

        }
        // Roles API
        [HttpPost("CreateRole")]
        public ActionResult CreateRole(RoleDto UserRequest)
        {

            return adminService.CreateRole(UserRequest);

        }

        [HttpPut("UpdateRole")]
        public Task<ActionResult> UpdateRole(RoleDto UserRequest)
        {

            return adminService.UpdateRole(UserRequest);

        }

        [HttpDelete("DeleteRoleById")]
        public ActionResult DeleteRoleById(string RoleId)
        {

            return adminService.DeleteRoleById(RoleId);

        }

        [HttpDelete("DeleteRolesById")]
        public ActionResult DeleteRolesById([FromBody] List<string> RoleId)
        {

            return adminService.DeleteRolesById(RoleId);

        }

        [HttpPost("GetRoles")]
        public Task<ActionResult> GetRoles(RolesFilter? parameter, int skip, int take, string? sortBy, string? sortOrder)
        {

            return adminService.GetRoles(parameter, skip, take, sortBy, sortOrder);

        }

        [HttpGet("GetRoleById")]
        public Task<ActionResult> GetRoleById(string RoleId)
        {

            return adminService.GetRoleById(RoleId);

        }

        [HttpGet("GetRoleByName")]
        public Task<ActionResult> GetRoleByName(string RoleName, int skiped, int take)
        {

            return adminService.GetRoleByName(RoleName, skiped, take);

        }

        [HttpPost("CreatePermission")]
        public ActionResult CreatePermission(PermissionDto UserRequest)
        {

            return adminService.CreatePermission(UserRequest);

        }

        [HttpPut("UpdatePermission")]
        public ActionResult UpdatePermission(PermissionDto UserRequest)
        {

            return adminService.UpdatePermission(UserRequest);

        }

        [HttpDelete("DeletePermissionById")]
        public ActionResult DeletePermissionById(string PermissionId)
        {

            return adminService.DeletePermissionById(PermissionId);

        }

        [HttpDelete("DeletePermissionsById")]
        public ActionResult DeletePermissionsById([FromBody] List<string> PermissionId)
        {

            return adminService.DeletePermissionsById(PermissionId);

        }

        [HttpGet("GetPermissions")]
        public Task<ActionResult> GetPermissions(int skiped, int take)
        {

            return adminService.GetPermissions(skiped, take);

        }

        [HttpGet("GetPermissionById")]
        public Task<ActionResult> GetPermissionById(string PermissionId)
        {

            return adminService.GetPermissionById(PermissionId);

        }

        [HttpGet("GetPermissionByName")]
        public Task<ActionResult> GetPermissionByName(string PermissionName, int skiped, int take)
        {

            return adminService.GetPermissionByName(PermissionName, skiped, take);

        }


    }
}