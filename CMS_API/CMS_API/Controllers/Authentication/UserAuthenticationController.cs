using CMS_API_Application.Dto.Authentication;
using CMS_API_Application.Interfaces.Servises.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CMS_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthenticationController(IAuthUserService authService
        ) : ControllerBase
    {


        [HttpPost("register")]
        public ActionResult RegisterUser(RegisterDto UserRequest)
        {

            return Ok(authService.Register(UserRequest));

        }

        [HttpPost("login")]
        public ActionResult Login(RequestLogInDto UserRequest)
        {

            return Ok(authService.Login(UserRequest));

        }

        [HttpPost("EmailVerification")]
        public ActionResult EmailVerification(VerificationEmailDto UserRequest)
        {

            return Ok(authService.EmailVerification(UserRequest));

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


        [HttpPost("ForgetPassword")]
        public ActionResult ForgetPassword([FromBody] string UserRequest)
        {

            return Ok(authService.ForgetPassword(UserRequest));

        }


    }


}
