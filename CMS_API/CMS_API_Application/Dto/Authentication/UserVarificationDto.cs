using CMS_API_Core.DomainModels;

namespace CMS_API_Application.Dto.Authentication
{
    public record UserVarificationDto(string VerificationCode, User NewUser);
}
