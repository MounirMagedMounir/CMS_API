namespace CMS_API_Core.DomainModels.Article
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedById { get; set; }
        public virtual User? ApprovedBy { get; set; }
        public string? ParentId { get; set; }
        public virtual Comment? Parent { get; set; }
        public string ArticleId { get; set; }
        public virtual ContentArticle Article { get; set; }

    }
}
