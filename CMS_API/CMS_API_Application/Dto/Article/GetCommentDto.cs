namespace CMS_API_Application.Dto.Article
{
    public class GetCommentDto : BaseDto
    {
        public string Content { get; set; }
        public string ArticleId { get; set; }
        public string ParentId { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedById { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
