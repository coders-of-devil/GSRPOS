using Domain.Entities.CustomerMod;
using MainApp.ViewModels.CustomerModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.CustomerMap
{
    public static class CustomerAddressMapping
    {
        public static CustomerAddressViewModel ToViewModel(this CustomerAddress entity)
        {
            return new CustomerAddressViewModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                Label = entity.Label,
                FlatOrOffice = entity.FlatOrOffice,
                Street = entity.Street,
                City = entity.City,
                PostalCode = entity.PostalCode,
                Country = entity.Country,
                IsDefault = entity.IsDefault,
                IsActive = entity.IsActive
            };
        }

        public static CustomerAddress ToEntity(this CustomerAddressViewModel vm)
        {
            return new CustomerAddress
            {
                Id = vm.Id,
                CustomerId = vm.CustomerId,
                Label = vm.Label,
                FlatOrOffice = vm.FlatOrOffice,
                Street = vm.Street,
                City = vm.City,
                PostalCode = vm.PostalCode,
                Country = vm.Country,
                IsDefault = vm.IsDefault,
                IsActive = vm.IsActive
            };
        }
    }
}
