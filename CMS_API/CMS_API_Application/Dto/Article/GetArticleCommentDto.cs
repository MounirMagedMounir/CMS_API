namespace CMS_API_Application.Dto.Article
{
    public class GetArticleCommentDto : BaseDto
    {
        public string Content { get; set; }
        public string? ParentId { get; set; }
        public List<GetArticleCommentDto> Children { get; set; } = new List<GetArticleCommentDto>();
    }
}
