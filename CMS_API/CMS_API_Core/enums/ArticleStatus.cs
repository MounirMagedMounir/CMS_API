namespace CMS_API_Core.enums
{
    public enum ArticleStatus
    {

        Draft, // The article is being written and edited, not yet ready for review.
        Pending, // The article is ready and waiting for review.
        Publish,// The article has been reviewed and approved for publishing.
        Review, // The article is under review by editors or peers.
        Rejected, // The article was rejected during the review process and will not be published. 

    }
}
