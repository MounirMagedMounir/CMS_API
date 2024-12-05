using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Article
{
    public class UpdateCommentDto
    {
        public string Id { get; set; }
        public bool IsApproved { get; set; }

        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsNullOrEmpty(Id, "Id", "Id Is Requered")
                       .IsTrueOrFalse(IsApproved, "IsApproved", "IsApproved Is Not a Boolean")
                       ;


            return validations.GetValidations();
        }
    }
}
