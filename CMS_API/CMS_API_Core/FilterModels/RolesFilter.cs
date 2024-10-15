using CMS_API_Core.DomainModels;

namespace CMS_API_Core.FilterModels
{
	public class RolesFilter : BaseFilterEntity
	{
		public string? Name { get; set; }
		public virtual ICollection<PermissionsFilter>? Permissions { get; set; }
	}
}
