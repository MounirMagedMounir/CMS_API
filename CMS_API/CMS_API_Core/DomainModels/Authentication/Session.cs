using System.ComponentModel.DataAnnotations;
using CMS_API_Core.Interfaces.Models;

namespace CMS_API_Core.DomainModels.Authentication
{
    public class Session : IBaseEntity
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddDays(15);
        public virtual RefreshToken RefreshToken { get; set; }
        public DateTime InActiveDate { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiryDate || DateTime.Now >= InActiveDate.AddMinutes(30) || !RefreshToken.IsActive;
        public string Browser { get; set; }
        public string Device { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}