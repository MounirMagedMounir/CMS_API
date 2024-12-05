using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Article
{
    public class CreateTagDto
    {

        public string Name { get; set; }
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
