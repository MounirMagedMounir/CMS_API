using CMS_API_Core.enums;
using CMS_API_Core.Validations;

namespace CMS_API_Core.FilterModels.Article
{
    public class ArticleFilter : BaseFilterEntity
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public DateTime? PublishDate { get; set; }
        public ArticleStatus? Status { get; set; }
        public int? ViewsCount { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public List<String?>? Tags { get; set; }


        public override List<string> Validation()
        {

            ModelValidations validations = new();

            validations
                       .IsString(Name, "Name", "Name Is Not a String")
                       .IsUsernameValid(Title, "Title", "Invalid Title : ")
                       ;


            return validations.GetValidations();
        }
    }
}
