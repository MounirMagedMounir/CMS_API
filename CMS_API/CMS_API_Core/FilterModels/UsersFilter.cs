using CMS_API_Core.DomainModels;
using CMS_API_Core.Validations;

namespace CMS_API_Core.FilterModels
{
    public class UsersFilter : BaseFilterEntity
    {
        public string? Name { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public bool? IsActive { get; set; }

        public string? RoleName { get; set; }

        public override List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsTrueOrFalse(IsActive, "IsActive", "IsActive must be (true) or (false)")
                       .IsString(Name, "Name", "Name Is Not a String")
                       ;


            return validations.GetValidations();
        }
    }
}
