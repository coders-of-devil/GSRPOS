using Domain.Entities.MenuMod;
using MainApp.ViewModels.MenuModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.MenuMap
{
    public static class KitchenOrderRoutingMapping
    {
        public static KitchenOrderRoutingViewModel ToViewModel(this KitchenOrderRouting entity)
        {
            return new KitchenOrderRoutingViewModel
            {
                Id = entity.Id,
                MenuItemId = entity.MenuItemId,
                MenuItemName = entity.MenuItem?.Name ?? string.Empty,
                KitchenDeviceId = entity.KitchenDeviceId,
                KitchenDeviceName = entity.KitchenDevice?.Name ?? string.Empty,
                Copies = entity.Copies
            };
        }

        public static KitchenOrderRouting ToEntity(this KitchenOrderRoutingViewModel vm)
        {
            return new KitchenOrderRouting
            {
                Id = vm.Id,
                MenuItemId = vm.MenuItemId,
                KitchenDeviceId = vm.KitchenDeviceId,
                Copies = vm.Copies
            };
        }
    }
}
