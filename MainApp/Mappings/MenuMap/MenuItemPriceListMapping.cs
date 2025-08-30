using Domain.Entities.MenuMod;
using MainApp.ViewModels.MenuModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MainApp.ViewModels.MenuModule.MenuItemPricelistViewModel;

namespace MainApp.Mappings.MenuMap
{
    public class MenuItemPriceListMapping
    {
        public static MenuItemPricelistViewModel ToVm(MenuItemPriceListEntry e) => new()
        {
            Id = e.Id,
            PriceListId = e.PriceListId,
            MenuItemId = e.MenuItemId,
            Price = e.Price,
            Notes = e.Notes ?? "",
            PriceListName = e.PriceList?.Name ?? "",
            MenuItemName = e.MenuItem?.Name ?? ""
        };

        public static MenuItemPriceListEntry ToEntity(MenuItemPricelistViewModel vm) => new()
        {
            Id = vm.Id,
            PriceListId = vm.PriceListId,
            MenuItemId = vm.MenuItemId,
            Price = vm.Price,
            Notes = vm.Notes
        };

    }
}
