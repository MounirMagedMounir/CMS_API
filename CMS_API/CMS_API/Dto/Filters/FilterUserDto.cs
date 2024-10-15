using CMS_API_Core.DomainModels;

namespace CMS_API.Dto.Filters
{
    public class FilterUserDto : BaseEntity
    {

        public string Name { get; set; }

        public string UserName { get; set; }



        public string Email { get; set; }



        public string Phone { get; set; }



        public string Password { get; set; }

        public string ProfileImage { get; set; }   

        public string Role { get; set; } 



        public bool IsActive { get; set; } 

    }
}
