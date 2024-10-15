using CMS_API.Dto.Authentication;
using CMS_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController(AuthService authService
        ) : ControllerBase
    {


        [HttpPost("register")]
        public ActionResult RegisterUser(RegisterDto UserRequest)
        {

            return authService.Register(UserRequest);

        }

        [HttpPost("login")]
        public ActionResult Login(LoginDto UserRequest)
        {

            return authService.Login(UserRequest);

        }

        [HttpPost("EmailVerification")]
        public ActionResult EmailVerification(VerificationEmailDto UserRequest)
        {

            return authService.EmailVerification(UserRequest);

        }

        [HttpPost("SignOut")]

        public ActionResult SignOut()
        {

            return authService.SignOut();

        }

        [HttpPost("RefreshToken")]
        public ActionResult RefreshToken([FromBody] string UserRequest)
        {

            return authService.RefreshToken(UserRequest);

        }


    }


}
