using CMS_API.Filters;
using CMS_API_Application.Dto.Article;
using CMS_API_Application.Interfaces.Servises.Article;
using CMS_API_Application.Services.Article;
using CMS_API_Core.FilterModels.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_API.Controllers.Article
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ArticleController(IArticleService _articleService
        ) : Controller
    {

        [HttpPost("Create")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Article_Create" })]
        public ActionResult Create(CreateArticleDto UserRequest)
        {

            return Ok(_articleService.Create(UserRequest));


        }

        [HttpPut("Update")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Article_Update" })]
        public ActionResult Update(UpdateArticleDto UserRequest)
        {

            return Ok(_articleService.Update(UserRequest));

        }

        [HttpPut("UpdateViewCount")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Article_UpdateViewCount" })]
        public ActionResult UpdateViewCount(string ArticleId)
        {

            return Ok(_articleService.UpdateViewCount(ArticleId));

        }

        [HttpDelete("DeleteById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Article_DeleteById" })]
        public ActionResult DeleteById(string ArticleId)
        {

            return Ok(_articleService.DeleteById(ArticleId));

        }

        [HttpPost("GetList")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Article_GetList" })]
        public ActionResult GetList(ArticleFilter? filterParameter, int skip, int take, string? sortBy, string? sortOrder)
        {

            return Ok(_articleService.GetList(filterParameter, skip, take, sortBy, sortOrder));

        }

        [HttpGet("GetById")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Article_GetById" })]
        public ActionResult GetById(string articleId)
        {

            return Ok(_articleService.GetById(articleId));

        }

        [HttpGet("GetListByUserId")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Article_GetListByUserId" })]
        public ActionResult GetListByUserId(string UserId, int skip, int take, string? sortBy, string? sortOrder)
        {

            return Ok(_articleService.GetListByUserId(UserId, skip, take, sortBy, sortOrder));

        }

        [HttpGet("GetCurrentUser")]
        [TypeFilter(typeof(PermissionFilterAttribute), Arguments = new object[] { "Article_GetCurrentUser" })]
        public ActionResult GetCurrentUser(int skip, int take, string? sortBy, string? sortOrder)
        {

            return Ok(_articleService.GetCurrentUser(skip, take, sortBy, sortOrder));

        }

    }
}
