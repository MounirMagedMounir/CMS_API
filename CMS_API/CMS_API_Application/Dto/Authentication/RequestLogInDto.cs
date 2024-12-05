using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Authentication
{
    public class RequestLogInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations
                       .IsNullOrEmpty(Email, "Email", "Email Is Requered")
                       .IsNullOrEmpty(Password, "Password", "Password Is Requered")
                       .IsEmailValid(Email, "Email", "Invalid Email :")
                       .IsPasswordValid(Password, "Password", "Invalid Password : ")
                       ;


            return validations.GetValidations();
        }
    }
}
