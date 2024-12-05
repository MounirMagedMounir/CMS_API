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
    public class CommentController(ICommentService _commentService) : ControllerBase
    {

        [HttpPost("Add")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Comment_Add" })]

        public ActionResult Add(AddCommentDto comment)
        {

            return Ok(_commentService.Add(comment));

        }

        [HttpPut("UpdateStat")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Comment_UpdateStat" })]

        public ActionResult UpdateStat(UpdateCommentDto comment)
        {

            return Ok(_commentService.UpdateStat(comment));

        }

        [HttpDelete("Delete")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Comment_Delete" })]

        public ActionResult Delete(string Id)
        {

            return Ok(_commentService.Delete(Id));

        }

        [HttpGet("GetByArticleId")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Comment_GetByArticleId" })]

        public ActionResult GetByArticleId(string articleId)
        {

            return Ok(_commentService.GetByArticleId(articleId));

        }

        [HttpGet("GetList")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Comment_GetList" })]

        public ActionResult GetList(CommentsFilter? filterParameter, int skip, int take, string sortBy, string sortOrder)
        {

            return Ok(_commentService.GetList(filterParameter, skip, take, sortBy, sortOrder));

        }

        [HttpGet("GetById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Comment_GetById" })]

        public ActionResult GetById(string Id)
        {

            return Ok(_commentService.GetById(Id));

        }

    }
}
