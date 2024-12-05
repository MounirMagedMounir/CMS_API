using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Article
{
    public class UpdateTagDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsNullOrEmpty(Id, "Id", "Id Is Requered")
                       .IsNullOrEmpty(Name, "Name", "Name Is Requered")
                       .IsString(Name, "Name", "Name Is Not a String")

                       ;


            return validations.GetValidations();
        }
    }
}
