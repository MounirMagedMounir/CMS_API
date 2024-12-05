using CMS_API_Core.enums;

namespace CMS_API_Application.Dto.Article
{
    public class ArticleContributorDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public ContributorRole ContributorRole { get; set; }
    }
}
