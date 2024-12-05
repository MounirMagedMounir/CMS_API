namespace CMS_API_Application.Interfaces.Servises
{
    public interface ISecurityService
    {
        bool HasPermissionByRole(Guid roleId, string permission);
    }


}
