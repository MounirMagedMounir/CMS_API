using CMS_API_Core.DomainModels;

namespace CMS_API.Dto.Filters
{
	public class FilterPermissionDto: BaseEntity
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
	}
}
