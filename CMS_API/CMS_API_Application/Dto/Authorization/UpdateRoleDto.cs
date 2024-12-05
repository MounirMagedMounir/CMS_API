using CMS_API_Core.Validations;

namespace CMS_API_Application.Dto.Authorization
{
    public class UpdateRoleDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UpdatePermissionDto>? Permissions { get; set; }
        public List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsNullOrEmpty(Id, "Id", "Id Is Requered")
                       .IsNullOrEmpty(Name, "Name", "Name Is Requered")
                       ;


            return validations.GetValidations();
        }
    }
}
