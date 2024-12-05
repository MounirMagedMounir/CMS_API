using CMS_API_Application.Dto.Authentication;
using CMS_API_Application.Interfaces.Servises.Authentication;
using CMS_API_Core.DomainModels;
using CMS_API_Core.DomainModels.Authentication;
using CMS_API_Core.helper.Response;
using CMS_API_Core.helper.Utils;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Core.Interfaces.Repository.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CMS_API.Services.Authentication
{
    public class AuthUserService(IConfiguration configuration
        , IHttpContextAccessor _httpContextAccessor
        , IUserRepository _userRepository
        , ISessionRepository _SessionRepository
        , IRefreshTokenRepository _RefreshTokenRepository
        , IMemoryCache _cache
        ) : IAuthUserService
    {

        readonly EmailSender emailSender = new(configuration);
        readonly JWT authentication = new(configuration, _httpContextAccessor);
        public ApiResponse<object?> Register(RegisterDto UserRequest)
        {

            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status400BadRequest,
                        message: validationErrors.ToList());
            }


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


            if (!UserRequest.Password.Equals(UserRequest.ConfirmPassword))
            {

                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status400BadRequest,
                        message: new List<string> { "Passwords dosen`t match", "make sure password and the Confirm Password match" });
            }


            if (_userRepository.IsEmailExists(UserRequest.Email))
            {
                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status400BadRequest,
                        message: [" Email Aleardy exist", "the email you provided alredy register"]);
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

        public ApiResponse<object?> Login(RequestLogInDto UserRequest)
        {
            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {
                return new ApiResponse<object?>(
                 data: null,
                 status: StatusCodes.Status400BadRequest,
                 message: validationErrors);


            }

            User User = _userRepository.GetUserByEmailAndPassword(UserRequest.Email, UserRequest.Password);

            if (User is null)
            {
                return new ApiResponse<object?>(
                          data: null,
                          status: StatusCodes.Status404NotFound,
                          message: ["Email or Password is incorrect", "make sure you put the right email and password"]);
            }

            var userSession = _SessionRepository.GetSessionByUserId(User.Id);

            if (userSession is not null)
            {
                _SessionRepository.DeleteSession(userSession);
            }

            var newSessionId = Guid.NewGuid().ToString();
            var newrefreshTokenId = Guid.NewGuid().ToString();
            RefreshToken newrefreshToken = new()
            {
                Id = newrefreshTokenId,
                SessionId = newSessionId,
                CreatedDate = DateTime.Now,
                Token = authentication.GenerateRefreshToken(),
                ExpiresOn = DateTime.Now.AddDays(7),
                RevokedOn = null,
            };



            Session newSession = new()
            {
                Id = newSessionId,
                Token = authentication.GenerateToken(User),
                ExpiryDate = DateTime.Now.AddMinutes(5),
                Browser = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"],
                Device = System.Net.Dns.GetHostName(),
                InActiveDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                UserId = User.Id,

            };

            _RefreshTokenRepository.AddRefreshToken(newrefreshToken);
            _SessionRepository.AddSession(newSession);


            _SessionRepository.SaveChanges();

            return new ApiResponse<object?>(
                             data: new List<object?> // Use object-compatible list for success
                             {
                                new ResponseLogInDto
                                {
                                    Token = newSession.Token,
                                    RefreshToken = newrefreshToken.Token
                                }
                             },
                             status: StatusCodes.Status200OK,
                             message: ["Login successful"]);
        }

        public ApiResponse<object?> EmailVerification(VerificationEmailDto Code)
        {
            var validationErrors = Code.Validation();
            if (validationErrors.Count > 0)
            {
                { }; return new ApiResponse<object?>(
                   data: null,
                   status: StatusCodes.Status400BadRequest,
                   message: validationErrors.ToList());
            }

            if (_cache.Get(Code.Email) is not null)
            {

                var NewUserRequst = _cache.Get<UserVarificationDto>(Code.Email);


                if (Code.VerificationCode != NewUserRequst.VerificationCode)
                {

                    return new ApiResponse<object?>(
                           data: null,
                           status: StatusCodes.Status400BadRequest,
                           message: ["The Verification Code is Wrong", "make sure you put the right Verification Code"]);

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
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedbyId = NewUserId,

                };

                _userRepository.AddUser(NewUser);

                _userRepository.SaveChanges();

                _cache.Remove(Code.Email);


                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status201Created,
                        message: ["Your Account Craeted successfuly"]);
            }
            else
            {

                return new ApiResponse<object?>(
                      data: null,
                      status: StatusCodes.Status404NotFound,
                      message: ["The Verification Code is Expird", "Register again "]);

            }
        }

        public ApiResponse<object?> SignOut()
        {

            var UserRequest = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var userSession = _SessionRepository.GetSessionByToken(UserRequest);

            if (userSession is null)
            {
                return new ApiResponse<object?>(
                            data: null,
                            status: StatusCodes.Status400BadRequest,
                            message: ["Token is Wrong"]);
            }

            _SessionRepository.DeleteSession(userSession);

            _SessionRepository.SaveChanges();

            return new ApiResponse<object?>(
                 data: null,
                 status: StatusCodes.Status200OK,
                 message: ["You have Been Signed Out"]);
        }

        public ApiResponse<object?> RefreshToken(string UserRequest)
        {

            var OldToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var userSession = _SessionRepository.GetSessionByToken(OldToken);
            var userRefreshToken = _RefreshTokenRepository.GetRefreshToken(UserRequest);

            if (userSession is null)
            {
                return new ApiResponse<object?>(
                 data: null,
                 status: StatusCodes.Status400BadRequest,
                 message: ["Invalid token"]);
            }

            if (userRefreshToken is null)
            {
                return new ApiResponse<object?>(
            data: null,
            status: StatusCodes.Status400BadRequest,
            message: ["Refresh Token is Wrong"]);
            }

            if (userSession.IsExpired)
            {
                if (userSession.IsExpired)
                    return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status401Unauthorized,
                    message: ["Token is expired", "Log in again"]);

            }

            userSession.InActiveDate = DateTime.Now;
            userSession.Token = authentication.GenerateToken(userSession.User);
            userSession.ExpiryDate = DateTime.Now.AddMinutes(5);
            userRefreshToken.Token = authentication.GenerateRefreshToken();
            userSession.LastUpdatedDate = DateTime.Now;

            _SessionRepository.SaveChanges();

            return new ApiResponse<object?>(
          data: new List<object?> // Use object-compatible list for success
          {
                                new ResponseLogInDto
                                {
                                Token = userSession.Token,
                                RefreshToken = userRefreshToken.Token
                                }
          },
          status: StatusCodes.Status200OK,
          message: ["Token refreshed successfully"]);
        }

        public ApiResponse<object?> ForgetPassword(string UserRequest)
        {

            if (!_userRepository.IsEmailExists(UserRequest))
            {
                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status401Unauthorized,
                        message: ["Email is not Exist", "make sure you put the right email"]);
            }
            var newPassword = Guid.NewGuid().ToString().Substring(0, 6);
            return emailSender.EmailMessage(UserRequest, "Forget Password", $"Hi {UserRequest} " +
                      $"  {newPassword}   " +
                      "                                                                " +
                      " is your new Password .");



        }
    }
}
