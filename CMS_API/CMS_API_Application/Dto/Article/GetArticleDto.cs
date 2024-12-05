using CMS_API_Core.enums;

namespace CMS_API_Application.Dto.Article
{
    public class GetArticleDto : BaseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Title { get; set; }
        public DateTime? PublishDate { get; set; }
        public ArticleStatus Status { get; set; } = ArticleStatus.Draft;
        public int ViewsCount { get; set; } = 0;
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string? Video { get; set; }
        public IEnumerable<GetArticleCommentDto?>? Comments { get; set; }
        public IEnumerable<GetTagDto?>? Tags { get; set; }
        public virtual ICollection<ArticleContributorDto> Contributors { get; set; }
    }
}
