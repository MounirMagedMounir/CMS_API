using CMS_API.Filters;
using CMS_API_Application.Dto;
using CMS_API_Application.Dto.Admin;
using CMS_API_Application.Interfaces.Servises;
using CMS_API_Core.FilterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserService userService) : Controller
    {


        [HttpPost("Create")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "User_Create" })]
        public ActionResult Create(CreateUserDto UserRequest)
        {

            return Ok(userService.Create(UserRequest));

        }

        [HttpPut("Update")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "User_Update" })]
        public ActionResult Update(UpdateUserDto UserRequest)
        {

            return Ok(userService.Update(UserRequest));

        }

        [HttpDelete("DeletePermanent")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "User_DeletePermanent" })]
        public ActionResult DeletePermanent(string UserId)
        {

            return Ok(userService.DeletePermanent(UserId));

        }

        [HttpDelete("Delete")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "User_Delete" })]
        public ActionResult Delete(string UserId)
        {

            return Ok(userService.Delete(UserId));

        }

        [HttpPost("GetList")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Users_GetList" })]
        public ActionResult GetList(UsersFilter? filterParameter, string? sortBy, string? sortOrder, int skip = 1, int take = 3)
        {

            return Ok(userService.GetList(filterParameter, skip, take, sortBy, sortOrder));

        }

        [HttpGet("GetCurrent")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "User_GetCurrent" })]
        public ActionResult GetCurrent()
        {

            return Ok(userService.GetCurrent());

        }

        [HttpGet("Search")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Users_Search" })]
        public ActionResult Search(string parameter, int skip, int take)
        {

            return Ok(userService.Search(parameter, skip, take));

        }

        [HttpGet("GetById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "User_GetById" })]
        public ActionResult GetById(string UserId)
        {

            return Ok(userService.GetById(UserId));

        }

    }
}
