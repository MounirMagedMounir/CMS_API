using CMS_API_Core.Interfaces.Models;

namespace CMS_API_Core.DomainModels
{
    public class BaseEntity : IBaseEntity
    {
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

        public string CreatedbyId { get; set; }
        public virtual User CreatedBy { get; set; }

        public string LastUpdatedbyId { get; set; }
        public virtual User? LastUpdatedBy { get; set; }


    }
}
