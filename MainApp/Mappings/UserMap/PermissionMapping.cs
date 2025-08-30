using Domain.Entities.UserMod;
using MainApp.ViewModels.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.UserMap
{
    public class PermissionMapping
    {
        public static PermissionViewModel ToViewModel(Permission p)
        {
            return new PermissionViewModel
            {
                Id = p.Id,
                PermissionGroupId = p.PermissionGroupId,
                GroupName = p.PermissionGroup?.Name ?? "",
                Action = p.Action,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            };
        }

        public static Permission ToDomainModel(PermissionViewModel vm)
        {
            return new Permission
            {
                Id = vm.Id,
                PermissionGroupId = vm.PermissionGroupId,
                Action = vm.Action,
                Description = vm.Description
            };
        }
    }
}
