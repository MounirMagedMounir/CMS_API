using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.FilterModels.Article;

namespace CMS_API_Core.Interfaces.Repository.Article
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetTags(TagsFilter filterParameter, string sortBy, string sortOrder);
        Tag GetTagById(string id);
        Tag GetTagByName(string tagName);
        IEnumerable<Tag> GetAllTags();
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(string id);
        bool IsTagExists(string id);
        void SaveChanges();


    }
}
