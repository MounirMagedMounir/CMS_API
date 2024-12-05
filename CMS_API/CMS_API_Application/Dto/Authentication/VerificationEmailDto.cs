using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Authentication
{
    public class VerificationEmailDto
    {
        public string VerificationCode { get; set; }

        public string Email { get; set; }
        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsNullOrEmpty(Email, "Email", "Email Is Requered")
                       .IsNullOrEmpty(VerificationCode, "VerificationCode", "VerificationCode Is Requered")
                       .IsEmailValid(Email, "Email", "Invalid Email :")

                       ;


            return validations.GetValidations();
        }
    }
}
