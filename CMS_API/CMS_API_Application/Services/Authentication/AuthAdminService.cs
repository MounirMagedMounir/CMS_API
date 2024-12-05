using CMS_API_Core.Interfaces.Repository.Authentication;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Core.DomainModels.Authentication;
using CMS_API_Core.helper.Utils;
using CMS_API_Core.helper.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using CMS_API_Application.Dto.Authentication;
using CMS_API_Application.Interfaces.Servises.Authentication;

namespace CMS_API.Services.Authentication
{
    public class AuthAdminService(
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IRefreshTokenRepository refreshTokenRepository) : IAuthAdminService
    {
        private readonly JWT _authentication = new(configuration, httpContextAccessor);

        public ApiResponse<object?> Login(RequestLogInDto userRequest)
        {
            var validationErrors = userRequest.Validation();

            if (validationErrors.Count > 0)
                return new ApiResponse<object?>(
              data: null,
              status: StatusCodes.Status400BadRequest,
              message: validationErrors);

            var user = userRepository.GetUserByEmailAndPassword(userRequest.Email, userRequest.Password);

            if (user is null)
                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status404NotFound,
                        message: new List<string> { "Email or Password is incorrect" });

            if (!userRepository.IsUserRoleAdmin(user.Id))
                return new ApiResponse<object?>(
                            data: null,
                            status: StatusCodes.Status403Forbidden,
                            message: new List<string> { "You are not authorized to log in here" });

            var userSession = sessionRepository.GetSessionByUserId(user.Id);

            if (userSession is not null)
            {
                sessionRepository.DeleteSession(userSession);
            }

            var newSession = new Session
            {
                Id = Guid.NewGuid().ToString(),
                Token = _authentication.GenerateToken(user),
                ExpiryDate = DateTime.Now.AddMinutes(5),
                InActiveDate = DateTime.Now,
                Browser = httpContextAccessor.HttpContext.Request.Headers["User-Agent"],
                Device = Environment.MachineName,
                CreatedDate = DateTime.Now,
                UserId = user.Id
            };

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                SessionId = newSession.Id,
                Token = _authentication.GenerateRefreshToken(),
                CreatedDate = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(7)
            };

            sessionRepository.AddSession(newSession);
            refreshTokenRepository.AddRefreshToken(newRefreshToken);
            sessionRepository.SaveChanges();

            return new ApiResponse<object?>(
                             data:

                             [
                                new ResponseLogInDto
                                {
                                    Token = newSession.Token,
                                    RefreshToken = newRefreshToken.Token
                                }
                             ],
                             status: StatusCodes.Status200OK,
                             message: new List<string> { "Login successful" });
        }

        public ApiResponse<object?> RefreshToken(string refreshToken)
        {
            var oldToken = _authentication.GetBearerToken();
            var userSession = sessionRepository.GetSessionByToken(oldToken);
            var userRefreshToken = refreshTokenRepository.GetRefreshToken(refreshToken);

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
                return new ApiResponse<object?>(
                data: new List<string> { "Log in again" },
                status: StatusCodes.Status401Unauthorized,
                message: new List<string> { "Token is expired" });


            userSession.InActiveDate = DateTime.Now;
            userSession.Token = _authentication.GenerateToken(userSession.User);
            userSession.ExpiryDate = DateTime.Now.AddMinutes(5);
            userRefreshToken.Token = _authentication.GenerateRefreshToken();
            userSession.LastUpdatedDate = DateTime.Now;

            sessionRepository.SaveChanges();

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
                 message: new List<string> { "Token refreshed successfully" });
        }

        public ApiResponse<object?> SignOut()
        {
            var bearerToken = _authentication.GetBearerToken();
            var userSession = sessionRepository.GetSessionByToken(bearerToken);

            if (userSession is null)
                return new ApiResponse<object?>(
                       data: null,
                       status: StatusCodes.Status400BadRequest,
                       message: new List<string> { "Inbalid token" });

            sessionRepository.DeleteSession(userSession);
            sessionRepository.SaveChanges();


            return new ApiResponse<object?>(
                             data: null,
                             status: StatusCodes.Status200OK,
                             message: new List<string> { "Sign-out successful" });
        }




    }
}
