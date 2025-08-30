using Domain.Entities.UserMod;
using MainApp.ViewModels.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.UserMap
{
    public class RoleMapping
    {
        public static RoleViewModel ToViewModel(Role r)
        {
            if (r is null) return null!;

            return new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name,
                IsActive = r.IsActive,
                IsSystemRole = r.IsSystemRole,
                IsPermissionAssigned = r.IsPermissionAssigned,
                CreatedAt = r.CreatedAt
            };
        }

        public static Role ToDomainModel(RoleViewModel vm)
        {
            if (vm is null) return null!;

            return new Role
            {
                Id = vm.Id,
                Name = vm.Name,
                IsActive = vm.IsActive,
                IsSystemRole = vm.IsSystemRole,
                IsPermissionAssigned = vm.IsPermissionAssigned
            };
        }
    }
}
