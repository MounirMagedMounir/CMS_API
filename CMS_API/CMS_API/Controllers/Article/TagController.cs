using CMS_API.Filters;
using CMS_API.Services.Article;
using CMS_API_Application.Dto.Article;
using CMS_API_Application.Interfaces.Servises.Article;
using CMS_API_Core.FilterModels.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_API.Controllers.Article
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagController(ITagService _tagService) : ControllerBase
    {


        [HttpPost("Create")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Tag_Create" })]
        public ActionResult Create(CreateTagDto tag)
        {
            return Ok(_tagService.Create(tag));
        }

        [HttpPut("Update")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Tag_Update" })]
        public ActionResult Update(UpdateTagDto tag)
        {
            return Ok(_tagService.Update(tag));
        }

        [HttpDelete("Delete")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Tag_Delete" })]
        public ActionResult Delete(string Id)
        {
            return Ok(_tagService.Delete(Id));
        }

        [HttpDelete("DeleteList")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Tag_DeleteList" })]
        public ActionResult DeleteList(List<string> Ids)
        {
            return Ok(_tagService.DeleteList(Ids));
        }

        [HttpGet("GetList")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Tag_GetList" })]
        public ActionResult GetList(TagsFilter filterParameter, int skip, int take, string sortBy, string sortOrder)
        {
            return Ok(_tagService.GetList(filterParameter, skip, take, sortBy, sortOrder));
        }

        [HttpGet("GetById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Tag_GetById" })]
        public ActionResult GetById(string Id)
        {
            return Ok(_tagService.GetById(Id));
        }

        [HttpGet("GetAll")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Tag_GetAll" })]
        public ActionResult GetAll(int skip, int take)
        {
            return Ok(_tagService.GetAll(skip, take));
        }


    }
}
