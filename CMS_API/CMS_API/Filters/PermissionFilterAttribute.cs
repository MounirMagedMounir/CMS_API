using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CMS_API_Application.Interfaces.Servises;

namespace CMS_API.Filters
{
    public class PermissionFilterAttribute : ActionFilterAttribute
    {
        private readonly string _permission;
        private readonly ISecurityService _securityService;

        public PermissionFilterAttribute(string permission, ISecurityService securityService)
        {
            _permission = permission;
            _securityService = securityService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var roleIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            if (roleIdClaim == null || !_securityService.HasPermissionByRole(Guid.Parse(roleIdClaim), _permission))
            {
                context.Result = new ForbidResult();
            }

            base.OnActionExecuting(context);
        }
    }

}
