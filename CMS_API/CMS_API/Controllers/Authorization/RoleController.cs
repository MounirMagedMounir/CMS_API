using CMS_API.Filters;
using CMS_API.Services.Authorization;
using CMS_API_Application.Dto.Authorization;
using CMS_API_Application.Interfaces.Servises.Authorization;
using CMS_API_Core.FilterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_API.Controllers.Authorization
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class RoleController(IRoleService _roleService, IRolePermissionService _rolePermissionService) : Controller
    {
        [HttpPost("Create")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Role_Create" })]
        public ActionResult Create(CreateRoleDto UserRequest)
        {

            return Ok(_roleService.Create(UserRequest));

        }

        [HttpPost("AddPermissions")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Role_AddPermissions" })]
        public ActionResult AddPermissions(RolepermissionDto UserRequest)
        {

            return Ok(_rolePermissionService.AddPermissions(UserRequest));

        }

        [HttpPut("Update")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Role_Update" })]
        public ActionResult Update(UpdateRoleDto UserRequest)
        {

            return Ok(_roleService.Update(UserRequest));

        }

        [HttpDelete("DeleteById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Role_DeleteById" })]
        public ActionResult DeleteById(string RoleId)
        {

            return Ok(_roleService.DeleteById(RoleId));

        }

        [HttpDelete("RemovePermissions")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Role_RemovePermissions" })]
        public ActionResult RemovePermissions(RolepermissionDto UserRequest)
        {

            return Ok(_rolePermissionService.RemovePermissions(UserRequest));

        }

        [HttpDelete("DeleteListById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Role_DeleteListById" })]
        public ActionResult DeleteListById([FromBody] List<string> RoleId)
        {

            return Ok(_roleService.DeleteListById(RoleId));

        }

        [HttpPost("GetList")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Role_GetList" })]
        public ActionResult GetList(RolesFilter? filterParameter, int skip, int take, string? sortBy, string? sortOrder)
        {

            return Ok(_roleService.GetList(filterParameter, skip, take, sortBy, sortOrder));

        }

        [HttpGet("GetById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Role_GetById" })]
        public ActionResult GetById(string RoleId)
        {

            return Ok(_roleService.GetById(RoleId));

        }

    }
}
