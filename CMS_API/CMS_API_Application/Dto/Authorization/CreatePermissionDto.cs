using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Authorization
{
    public class CreatePermissionDto
    {
        public string Name { get; set; }
        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsNullOrEmpty(Name, "Name", "Name Is Requered")


                       ;


            return validations.GetValidations();
        }

    }
}
