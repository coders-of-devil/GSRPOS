using Domain.Entities.MenuMod;
using MainApp.ViewModels.MenuModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.MenuMap
{
    public static class KitchenMapping
    {
        public static KitchenDeviceViewModel ToViewModel(this KitchenDevice entity)
        {
            return new KitchenDeviceViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type,
                ConnectionType = entity.ConnectionType,
                IpAddress = entity.IpAddress,
                Port = entity.Port,
                Model = entity.Model,
                PreparationAreaId = entity.PreparationAreaId,
                PreparationAreaName = entity.PreparationArea?.Name ?? string.Empty,
                IsActive = entity.IsActive,
                DefaultCopies = entity.DefaultCopies
            };
        }

        public static KitchenDevice ToEntity(this KitchenDeviceViewModel vm)
        {
            return new KitchenDevice
            {
                Id = vm.Id,
                Name = vm.Name,
                Type = vm.Type,
                ConnectionType = vm.ConnectionType,
                IpAddress = vm.IpAddress,
                Port = vm.Port,
                Model = vm.Model,
                PreparationAreaId = vm.PreparationAreaId,
                IsActive = vm.IsActive,
                DefaultCopies = vm.DefaultCopies
            };
        }
    }
}
