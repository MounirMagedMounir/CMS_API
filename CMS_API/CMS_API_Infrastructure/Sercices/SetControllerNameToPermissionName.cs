using CMS_API_Application.Interfaces.Servises;
using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.Interfaces.Repository.Authorization;
using System.Runtime.ConstrainedExecution;


namespace CMS_API_Infrastructure.Sercices
{
    public class SetControllerNameToPermissionName(IRolePermissionRepository _rolepermissionRepository, IPermissionRepository _PermissionRepository) : ISetControllerNameToPermissionName
    {

        public void SetPermission(string permission)
        {
            Permission per = _PermissionRepository.GetPermissionByName(permission);

            var NewPermissionId = per?.Id ?? Guid.NewGuid().ToString();

            if (per is null)
            {

                Permission NewPermission = new()
                {
                    Id = NewPermissionId,
                    Name = permission,
                    LastUpdatedbyId = "34e53975-bf4e-43b9-baec-629b28a12e14",
                    LastUpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedbyId = "34e53975-bf4e-43b9-baec-629b28a12e14"
                };
                _PermissionRepository.AddPermission(NewPermission);
            }
            if (!_rolepermissionRepository.HasPermission("12483a8a-ee39-4812-b02e-d9575dde6c65", permission))
            {
                RolePermission rolepermission = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    RoleId = "12483a8a-ee39-4812-b02e-d9575dde6c65",
                    PermissionId = NewPermissionId
                };
                _rolepermissionRepository.AddRolePermission(rolepermission);
                _PermissionRepository.SaveChanges();
            }
        }


    }
}

