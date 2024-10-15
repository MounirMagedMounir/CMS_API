using CMS_API_Core.DomainModels;
using CMS_API_Core.FilterModels;

namespace CMS_API.Dto.Filters
{
	public class FilterRoleDto : BaseEntity
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public virtual ICollection<FilterPermissionDto>? Permissions { get; set; }
	}
}
