using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; } = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png";
        public bool IsActive { get; set; } = true;
        public string Role { get; set; } = "ba32b7a9-8f0e-40ea-85d9-836a643ba13c";
        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsNullOrEmpty(Id, "Id", "Id Is Requered")
                       .IsNullOrEmpty(Name, "Name", "Name Is Requered")
                       .IsNullOrEmpty(Email, "Email", "Email Is Requered")
                       .IsNullOrEmpty(Phone, "Phone", "Phone Is Requered")
                       .IsNullOrEmpty(UserName, "UserName", "UserName Is Requered")
                       .IsNullOrEmpty(Password, "Password", "Password Is Requered")

                       .IsTrueOrFalse(IsActive, "IsActive", "IsActive must be (true) or (false)")
                       .IsString(Name, "Name", "Name Is Not a String")
                       .IsPhoneValid(Phone, "Phone", "Invalid Phone : ")
                       .IsUsernameValid(UserName, "UserName", "Invalid username : ")
                       .IsEmailValid(Email, "Email", "Invalid Email :")
                       .IsPasswordValid(Password, "Password", "Invalid Password : ")
                       ;


            return validations.GetValidations();
        }
    }
}
