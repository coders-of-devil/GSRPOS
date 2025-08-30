using Domain.Entities.MenuMod;
using MainApp.ViewModels.MenuModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.MenuMap
{
    public class MenuItemTypeMapping
    {
        public static MenuItemTypeViewModel ToTypeViewModel(MenuItemType t) => new()
        {
            Id = t.Id,
            ItemId = t.ItemId,
            TypeName = t.TypeName,
            Price = t.Price,
            Description = t.Description ?? ""
        };

        public static MenuItemType ToTypeDomain(MenuItemTypeViewModel vm) => new()
        {
            Id = vm.Id,
            ItemId = vm.ItemId,
            TypeName = vm.TypeName,
            Price = vm.Price,
            Description = vm.Description
        };
    }
}
