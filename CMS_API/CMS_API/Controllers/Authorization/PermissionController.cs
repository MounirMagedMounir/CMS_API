using CMS_API.Filters;
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
    public class PermissionController(IPermissionService permissionService) : Controller
    {
        [HttpPost("Create")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Permission_Create" })]
        public ActionResult Create(CreatePermissionDto UserRequest)
        {

            return Ok(permissionService.Create(UserRequest));

        }

        [HttpPut("Update")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Permission_Update" })]
        public ActionResult Update(UpdatePermissionDto UserRequest)
        {

            return Ok(permissionService.Update(UserRequest));

        }

        [HttpDelete("DeleteById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Permission_DeleteById" })]
        public ActionResult DeleteById(string PermissionId)
        {

            return Ok(permissionService.DeleteById(PermissionId));

        }

        [HttpDelete("DeleteListById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Permission_DeleteListById" })]
        public ActionResult DeleteListById([FromBody] List<string> PermissionId)
        {

            return Ok(permissionService.DeleteListById(PermissionId));

        }

        [HttpGet("GetList")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Permission_GetList" })]
        public ActionResult GetList(PermissionsFilter filterParameter, int skip, int take, string sortBy, string sortOrder)
        {

            return Ok(permissionService.GetList(filterParameter, skip, take, sortBy, sortOrder));

        }

        [HttpGet("GetById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Permission_GetById" })]
        public ActionResult GetById(string PermissionId)
        {

            return Ok(permissionService.GetById(PermissionId));

        }

        [HttpGet("GetByName")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Permission_GetByName" })]
        public ActionResult GetByName(string PermissionName, int skip, int take)
        {

            return Ok(permissionService.GetByName(PermissionName, skip, take));

        }

        [HttpGet("GetNames")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Permission_GetNames" })]
        public ActionResult GetNames()
        {

            return Ok(permissionService.GetNames());

        }
    }
}
