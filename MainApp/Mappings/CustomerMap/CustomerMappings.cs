using Domain.Entities.CustomerMod;
using MainApp.ViewModels.CustomerModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.CustomerMap
{
    public static class CustomerMappings
    {
        public static CustomerViewModel ToViewModel(this Customer entity)
        {
            return new CustomerViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Phone = entity.Phone,
                Email = entity.Email,
                DOB = entity.DOB,
                Gender = entity.Gender,
                MembershipId = entity.MembershipId,
                MembershipName = entity.Membership?.Name ?? "",
                LoyaltyPoints = entity.LoyaltyPoints,
                CreditBalance = entity.CreditBalance,
                Source = entity.Source,
                Notes = entity.Notes
            };
        }

        public static Customer ToEntity(this CustomerViewModel vm)
        {
            return new Customer
            {
                Id = vm.Id,
                Name = vm.Name,
                Phone = vm.Phone,
                Email = vm.Email,
                DOB = vm.DOB,
                Gender = vm.Gender,
                MembershipId = vm.MembershipId,
                LoyaltyPoints = vm.LoyaltyPoints,
                CreditBalance = vm.CreditBalance,
                Source = vm.Source,
                Notes = vm.Notes
            };
        }
    }
}
