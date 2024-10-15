using CMS_API_Core.Interfaces.Models;

namespace CMS_API_Core.FilterModels
{
    public class BaseFilterEntity : IBaseEntity
    {
        public string? Id { get; set; }

        public DateTime? CreatedDateFrom { get; set; }

        public DateTime? CreatedDateTo { get; set; }

        public DateTime? LastUpdatedDateFrom { get; set; }

        public DateTime? LastUpdatedDateTo { get; set; }

        public string? CreatedbyId { get; set; }

        public string? LastUpdatedbyId { get; set; }

        public virtual extern Dictionary<string, string> Validation();


    }
}
