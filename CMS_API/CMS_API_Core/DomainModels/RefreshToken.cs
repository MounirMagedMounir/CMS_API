using CMS_API_Core.Interfaces.Models;

namespace CMS_API_Core.DomainModels
{
    public class RefreshToken : IBaseEntity
    {

        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiresOn;
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;

    }
}