using Domain.Entities.UserMod;
using MainApp.ViewModels.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.UserMap
{
    public class PermissionGroupMapping
    {
        public static PermissionGroupViewModel ToViewModel(PermissionGroup r)
        {
            if (r is null) return null!;

            return new PermissionGroupViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Code = r.Code,
                IsModelAssociated = r.IsModelAssociated,
                CreatedAt = r.CreatedAt
            };
        }

        public static PermissionGroup ToDomainModel(PermissionGroupViewModel vm)
        {
            if (vm is null) return null!;

            return new PermissionGroup
            {
                Id = vm.Id,
                Name = vm.Name,
                Code = vm.Code,
                IsModelAssociated = vm.IsModelAssociated
            };
        }
    }
}
