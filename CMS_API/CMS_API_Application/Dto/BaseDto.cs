namespace CMS_API_Application.Dto
{
    public class BaseDto
    {
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

        public string CreatedbyId { get; set; }
        public string CreatedByName { get; set; }

        public string LastUpdatedbyId { get; set; }
        public string LastUpdatedByName { get; set; }
    }
}
