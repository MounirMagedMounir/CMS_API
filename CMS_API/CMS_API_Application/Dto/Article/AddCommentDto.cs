using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Article
{
    public class AddCommentDto
    {
        public string Content { get; set; }
        public bool IsApproved { get; set; } = false;
        public string ArticleId { get; set; }
        public string? ParentId { get; set; }
        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsNullOrEmpty(Content, "Content", "Content Is Requered")
                       .IsNullOrEmpty(ArticleId, "ArticleId", "ArticleId Is Requered")

                       .IsTrueOrFalse(IsApproved, "IsApproved", "IsApproved Is Not a Boolean")
                       ;


            return validations.GetValidations();
        }
    }
}
