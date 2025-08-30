using Domain.Entities.UserMod;
using MainApp.ViewModels.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.UserMap
{
    public class UserMapping
    {
        public static UserViewModel ToViewModel(User r)
        {
            if (r is null) return null!;

            return new UserViewModel
            {
                Id = r.Id,
                Fullname = r.Fullname,
                PhoneNo = r.PhoneNo,
                Email = r.Email,
                Username = r.Username,
                IsActive = r.IsActive,
                IsLocked = r.IsLocked,
                LastLogin = r.LastLogin,
                CreatedAt = r.CreatedAt
            };
        }

        public static User ToDomainModel(UserViewModel vm)
        {
            if (vm is null) return null!;

            return new User
            {
                Id = vm.Id,
                Fullname = vm.Fullname,
                PhoneNo = vm.PhoneNo,
                Email = vm.Email,
                IsActive = vm.IsActive,
                IsLocked = vm.IsLocked
            };
        }
    }
}
