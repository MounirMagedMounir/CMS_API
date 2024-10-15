using CMS_API.Dto.Authentication;
using CMS_API.Dto.Filters;
using CMS_API_Core.DomainModels;
using CMS_API_Core.helper.Utils;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CMS_API.Services
{
    public class AuthService(IConfiguration configuration
        , IHttpContextAccessor _httpContextAccessor
        , IUserRepository _userRepository
        , ISessionRepository _SessionRepository
        , IRefreshTokenRepository _RefreshTokenRepository
        , IMemoryCache _cache
        ) : ControllerBase
    {

        readonly EmailSender emailSender = new(configuration);
        readonly Authentication authentication = new(configuration);
        public ActionResult Register(RegisterDto UserRequest)
        {
            var Respons = new Dictionary<string, object>();

            var NewUserId = Guid.NewGuid().ToString();

            User NewUser = new()
            {
                Id = NewUserId,
                Name = UserRequest.Name,
                Email = UserRequest.Email,
                UserName = UserRequest.UserName,
                Password = UserRequest.Password,
                Phone = UserRequest.Phone

            };


            if (NewUser.Validation().Count > 0)
            {
                return BadRequest(NewUser.Validation().ToList());

            }



            if (string.IsNullOrEmpty(UserRequest.ConfirmPassword))
            {
                Respons["ConfirmPassword"] = "ConfirmPassword Is Requered";
                return BadRequest(Respons.ToList());
            }
            else if (!UserRequest.Password.Equals(UserRequest.ConfirmPassword))
            {
                Respons["ConfirmPassword"] = "Passwords dosen`t match";
                return BadRequest(Respons.ToList());
            }





            if (_userRepository.IsEmailExists(UserRequest.Email))
            {
                Respons["Alert"] = " Email Aleardy exist";
                return BadRequest(Respons.ToList());
            }




            var VerificationCode = Guid.NewGuid().ToString().Substring(0, 6);

            _cache.Set(NewUser.Email, new UserVarificationDto(VerificationCode, NewUser), new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });


            return emailSender.EmailMessage(NewUser.Email, "Verification Code", $"Hi {NewUser.Name} " +
                  $"  {VerificationCode}   " +
                  "                                                                " +
                  " is your verification code .");


        }

        public ActionResult Login(LoginDto UserRequest)
        {
            var Respons = new Dictionary<string, object>();
            if (UserRequest.Email is null)
            {
                Respons["Email"] = " Email is Empty";
                return BadRequest(Respons.ToList());
            }
            if (UserRequest.Password is null)
            {
                Respons["Password"] = " Password is Empty";
                return BadRequest(Respons.ToList());
            }


            User User = _userRepository.GetUserByEmailAndPassword(UserRequest.Email, UserRequest.Password);

            if (User is null)
            {
                Respons["Alert"] = "Email or Password is Wrong";
                return NotFound(Respons.ToList());
            }
            var userSession = _SessionRepository.GetSessionByUserId(User.Id);
            if (userSession is not null)
            {
                //_context.Session.Remove(userSession);
                _RefreshTokenRepository.DeleteRefreshToken(userSession.RefreshToken);
            }


            var NewrefreshTokenId = Guid.NewGuid().ToString();
            RefreshToken NewrefreshToken = new()
            {
                Id = NewrefreshTokenId,
                CreatedDate = DateTime.Now,
                Token = authentication.GenerateRefreshToken(),
                ExpiresOn = DateTime.Now.AddDays(7),
                RevokedOn = null,
            };


            var NewSessionId = Guid.NewGuid().ToString();
            Session NewSession = new()
            {
                Id = NewSessionId,
                Token = authentication.GenerateToken(User),
                ExpiryDate = DateTime.Now.AddMinutes(5),
                Browser = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"],
                Device = System.Net.Dns.GetHostName(),
                RefreshTokenId = NewrefreshTokenId,
                InActiveDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                UserId = User.Id,

            };

            _RefreshTokenRepository.AddRefreshToken(NewrefreshToken);
            _SessionRepository.AddSession(NewSession);


            _RefreshTokenRepository.SaveChangesAsync();
            _SessionRepository.SaveChangesAsync();

            Respons["Success"] = "You have Loged In Successfuly";
            Respons["Token"] = NewSession.Token;
            Respons["RefreshToken"] = NewrefreshToken.Token;
            return Ok(Respons.ToList());
        }

        public ActionResult EmailVerification(VerificationEmailDto Code)
        {
            var Respons = new Dictionary<string, object>();
            if (_cache.Get(Code.Email) is not null)
            {

                var NewUserRequst = _cache.Get<UserVarificationDto>(Code.Email);


                if (Code.VerificationCode != NewUserRequst.VerificationCode)
                {
                    Respons["VerificationCode"] = "The Verification Code is Wrong";

                    return BadRequest(Respons.ToList());
                }

                var NewUserId = Guid.NewGuid().ToString();

                User NewUser = new()
                {
                    Id = NewUserId,
                    Name = NewUserRequst.NewUser.Name,
                    Email = NewUserRequst.NewUser.Email,
                    UserName = NewUserRequst.NewUser.UserName,
                    Password = NewUserRequst.NewUser.Password,
                    Phone = NewUserRequst.NewUser.Phone,
                    CreatedDate = DateTime.Now,
                    CreatedbyId = NewUserId,
                    LastUpdatedbyId = NewUserId,

                };

                _userRepository.AddUser(NewUser);

                _userRepository.SaveChangesAsync();

                _cache.Remove(Code.Email);
                Respons["Success"] = "Your Account Craeted successfuly";

                return Created(StatusCodes.Status201Created + "", Respons.ToList());
            }
            else
            {
                Respons["Alert"] = "The Verification Code is Expird";

                return BadRequest(Respons.ToList());

            }
        }

        public ActionResult SignOut()
        {

            var Respons = new Dictionary<string, object>();
            var UserRequest = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");


            Console.WriteLine("token fom api  " + UserRequest);
            var userSession = _SessionRepository.GetSessionByToken(UserRequest);
            if (userSession is null)
            {
                Respons["Alert"] = "Token is Wrong";
                return NotFound(Respons.ToList());
            }

            _RefreshTokenRepository.DeleteRefreshToken(userSession.RefreshToken);

            _RefreshTokenRepository.SaveChangesAsync();

            Respons["Success"] = "You have Been Signed Out";
            return Ok(Respons.ToList());
        }

        public ActionResult RefreshToken(string UserRequest)
        {

            var Respons = new Dictionary<string, object>();

            var OldToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var userSession = _SessionRepository.GetSessionByToken(OldToken);

            if (userSession is null)
            {
                Respons["Alert"] = "Token is Wrong";
                return NotFound(Respons.ToList());
            }

            if (userSession.IsExpired)
            {
                Respons["Alert"] = "Token is Expired";
                _SessionRepository.DeleteSession(userSession);
                _RefreshTokenRepository.DeleteRefreshToken(userSession.RefreshToken);
                return Unauthorized(Respons.ToList());
            }

            userSession.InActiveDate = DateTime.Now;
            userSession.Token = authentication.GenerateToken(userSession.User);
            userSession.ExpiryDate = DateTime.Now.AddMinutes(5);
            userSession.RefreshToken.Token = authentication.GenerateRefreshToken();
            userSession.LastUpdatedDate = DateTime.Now;

            _RefreshTokenRepository.SaveChangesAsync();
            _SessionRepository.SaveChangesAsync();

            Respons["Success"] = "Your token is Refreshed";
            Respons["Token"] = userSession.Token;
            Respons["RefreshToken"] = userSession.RefreshToken.Token;
            return Ok(Respons.ToList());
        }
    }
}
