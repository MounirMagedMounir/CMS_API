using CMS_API_Application.Dto.Authentication;
using CMS_API_Core.helper.Response;
using Microsoft.AspNetCore.Mvc;

namespace CMS_API_Application.Interfaces.Servises.Authentication
{
    public interface IAuthUserService : IAuthService
    {
        ApiResponse<object?> Register(RegisterDto UserRequest);

        ApiResponse<object?> EmailVerification(VerificationEmailDto Code);

        ApiResponse<object?> ForgetPassword(string UserRequest);

    }
}
