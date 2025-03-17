using CMS_API_Core.DomainModels;
using CMS_API_Core.Validations;
using System.Numerics;

namespace CMS_API_Core.FilterModels
{
    public class RolesFilter : BaseFilterEntity
    {
        public string? Name { get; set; }
        public virtual ICollection<RolePermissionFilter>? Permissions { get; set; }
        public override List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsString(Name, "Name", "Name Is Not a String");



            return validations.GetValidations();
        }
    }
}
