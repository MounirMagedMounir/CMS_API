using CMS_API_Core.DomainModels;
using CMS_API_Core.Validations;
using System.Numerics;

namespace CMS_API_Core.FilterModels
{
    public class RolesFilter : BaseFilterEntity
    {
        public string? Name { get; set; }
        public virtual ICollection<PermissionsFilter>? Permissions { get; set; }
        public override List<string> Validation()
        {

            ModelValidations validations = new();



            return validations.GetValidations();
        }
    }
}
