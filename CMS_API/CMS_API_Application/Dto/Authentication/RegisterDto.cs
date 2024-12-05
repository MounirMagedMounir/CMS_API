using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Authentication
{
    public class RegisterDto
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations
                       .IsNullOrEmpty(Name, "Name", "Name Is Requered")
                       .IsNullOrEmpty(Email, "Email", "Email Is Requered")
                       .IsNullOrEmpty(Phone, "Phone", "Phone Is Requered")
                       .IsNullOrEmpty(UserName, "UserName", "UserName Is Requered")
                       .IsNullOrEmpty(Password, "Password", "Password Is Requered")
                       .IsNullOrEmpty(ConfirmPassword, "ConfirmPassword", "ConfirmPassword Is Requered")

                       .IsString(Name, "Name", "Name Is Not a String")
                       .IsPhoneValid(Phone, "Phone", "Invalid Phone : ")
                       .IsUsernameValid(UserName, "UserName", "Invalid username : ")
                       .IsEmailValid(Email, "Email", "Invalid Email :")
                       .IsPasswordValid(Password, "Password", "Invalid Password : ")
                       .IsPasswordValid(ConfirmPassword, "ConfirmPassword", "Invalid ConfirmPassword : ")
                       ;


            return validations.GetValidations();
        }
    }
}
