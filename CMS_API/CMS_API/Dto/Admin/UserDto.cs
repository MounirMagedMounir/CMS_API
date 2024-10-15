namespace CMS_API.Dto.Admin
{
	public class UserDto
	{
		public string? Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string ProfileImage { get; set; } = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png";
		public bool IsActive { get; set; } = true;
		public string Role { get; set; }
	}
}
