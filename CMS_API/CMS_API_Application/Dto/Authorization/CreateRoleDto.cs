using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Authorization
{
    public class CreateRoleDto
    {

        public string Name { get; set; } 

        public virtual ICollection<CreatePermissionDto>? Permissions { get; set; }

        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsNullOrEmpty(Name, "Name", "Name Is Requered")
                       .IsString(Name, "Name", "Name Is Not a String")

                       ;


            return validations.GetValidations();
        }
    }
}
