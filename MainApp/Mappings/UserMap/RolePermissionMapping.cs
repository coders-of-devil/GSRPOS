using Domain.Entities.UserMod;
using MainApp.ViewModels.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.UserMap
{
    public class RolePermissionMapping
    {
        public static RolePermissionViewModel ToViewModel(RolePermission model)
        {
            if (model == null) return null;

            return new RolePermissionViewModel
            {
                Id = model.Id,
                RoleId = model.RoleId,
                RoleName = model.Role.Name ?? "",
                PermissionId = model.PermissionId,
                PermissionName = model.Permission.Description ?? "",
                CreatedAt = model.CreatedAt
            };
        }

        public static RolePermission ToDomainModel(RolePermissionViewModel model)
        {
            if (model == null) return null;

            return new RolePermission
            {
                Id = model.Id,
                RoleId = model.RoleId,
                PermissionId = model.PermissionId
            };
        }
    }
}
