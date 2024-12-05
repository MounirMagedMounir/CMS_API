using CMS_API_Core.DomainModels.Authentication;
using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.Validations;

namespace CMS_API_Core.DomainModels
{
    public class User : BaseEntity

    {

        public string Name { get; set; }

        public string UserName { get; set; }



        public string Email { get; set; }



        public string Phone { get; set; }



        public string Password { get; set; }



        public string ProfileImage { get; set; } = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png";



        public bool IsActive { get; set; } = true;

        public string RoleId { get; set; } = "ba32b7a9-8f0e-40ea-85d9-836a643ba13c";

        public virtual Role Role { get; set; }

        public virtual Session? Session { get; set; }

        public virtual ICollection<ContentArticle> Articles { get; set; }



    }
}
