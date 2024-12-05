using CMS_API_Core.enums;
using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Article
{
    public class UpdateArticleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public ArticleStatus Status { get; set; } = ArticleStatus.Draft;
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string? Video { get; set; }
        public IEnumerable<UpdateTagDto?>? Tags { get; set; }
        public virtual ICollection<ArticleContributorDto> Contributors { get; set; }
        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsNullOrEmpty(Id, "Id", "Id Is Requered")
                       .IsNullOrEmpty(Name, "Name", "Name Is Requered")
                       .IsNullOrEmpty(Title, "Title", "Title Is Requered")
                       .IsNullOrEmpty(Description, "Description", "Description Is Requered")
                       .IsNullOrEmpty(Content, "Content", "Content Is Requered")
                       .IsNullOrEmpty(Image, "Image", "Image Is Requered")

                       .IsString(Name, "Name", "Name Is Not a String")

                       ;


            return validations.GetValidations();
        }
    }
}
