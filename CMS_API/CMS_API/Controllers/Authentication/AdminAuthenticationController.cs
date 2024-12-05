using CMS_API_Application.Dto.Authentication;
using CMS_API_Application.Interfaces.Servises.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CMS_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAuthenticationController(IAuthAdminService authService
        ) : ControllerBase
    {



        [HttpPost("login")]
        public ActionResult Login(RequestLogInDto UserRequest)
        {

            return Ok(authService.Login(UserRequest));

        }

        [HttpPost("SignOut")]

        public ActionResult SignOut()
        {

            return Ok(authService.SignOut());

        }

        [HttpPost("RefreshToken")]
        public ActionResult RefreshToken([FromBody] string UserRequest)
        {

            return Ok(authService.RefreshToken(UserRequest));

        }

    }
}
