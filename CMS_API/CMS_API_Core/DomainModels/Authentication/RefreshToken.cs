using CMS_API_Core.Interfaces.Models;

namespace CMS_API_Core.DomainModels.Authentication
{
    public class RefreshToken : IBaseEntity
    {

        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; } = DateTime.Now.AddDays(15);
        public bool IsExpired => DateTime.Now >= ExpiresOn;
        public DateTime? RevokedOn { get; set; } = null;
        public bool IsActive => (DateTime.Now <= RevokedOn || RevokedOn == null) && !IsExpired;

        public string SessionId { get; set; }
        public Session Session { get; set; }
    }
}