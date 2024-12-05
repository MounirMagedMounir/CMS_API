using CMS_API_Application.Dto.Authentication;
using CMS_API_Core.helper.Response;

namespace CMS_API_Application.Interfaces.Servises.Authentication
{
    public interface IAuthService
    {
        ApiResponse<object?> Login(RequestLogInDto userRequest);
        ApiResponse<object?> SignOut();
        ApiResponse<object?> RefreshToken(string refreshToken);
    }


}
