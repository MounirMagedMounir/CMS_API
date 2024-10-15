using System.ComponentModel.DataAnnotations;
using CMS_API_Core.Interfaces.Models;

namespace CMS_API_Core.DomainModels
{
    public class Session : IBaseEntity
	{
		public Session() { }
		public string Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
		public string Token { get; set; }
		public DateTime ExpiryDate { get; set; }
		public string RefreshTokenId { get; set; }
		public virtual RefreshToken RefreshToken { get; set; }
		public DateTime InActiveDate { get; set; }
		public bool IsExpired => DateTime.Now >= CreatedDate.AddHours(8) || DateTime.Now >= InActiveDate.AddMinutes(30) || RefreshToken.IsExpired;
		public string Browser { get; set; }
		public string Device { get; set; }
		public string UserId { get; set; }
		public virtual User User { get; set; }
	}
}