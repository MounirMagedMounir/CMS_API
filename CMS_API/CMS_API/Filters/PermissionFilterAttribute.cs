using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CMS_API_Application.Interfaces.Servises;
using CMS_API_Infrastructure.Sercices;

namespace CMS_API.Filters
{
    public class PermissionFilterAttribute : ActionFilterAttribute
    {
        private readonly string _permission;
        private readonly ISecurityService _securityService;
        private readonly ISetControllerNameToPermissionName _setControllerNameToPermissionName;

        public PermissionFilterAttribute(string permission, ISecurityService securityService, ISetControllerNameToPermissionName setControllerNameToPermissionName)
        {
            _permission = permission;
            _securityService = securityService;
            _setControllerNameToPermissionName = setControllerNameToPermissionName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var roleIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            _setControllerNameToPermissionName.SetPermission(_permission);

            if (roleIdClaim == null || !_securityService.HasPermissionByRole(Guid.Parse(roleIdClaim), _permission))
            {
                context.Result = new ForbidResult();
            }

            base.OnActionExecuting(context);
        }



    }

}
