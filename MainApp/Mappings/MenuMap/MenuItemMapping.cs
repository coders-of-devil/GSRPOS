using MainApp.ViewModels.MenuModule;
using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.MenuMap
{
    public class MenuItemMapping
    {
        public static MenuItemViewModel ToViewModel(Domain.Entities.MenuMod.MenuItem e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description ?? "",
            CategoryId = e.CategoryId,
            CategoryName = e.Category?.Name ?? "",
            StockKeepingUnit = e.StockKeepingUnit ?? "",
            TaxRate = e.TaxRate,
            Barcode = e.Barcode ?? "",
            ImagePath = e.ImagePath ?? "",
            DisplayOrder = e.DisplayOrder,
            ExpectedPreparationTime = e.ExpectedPreparationTime,
            PreparationAreaId = e.PreparationAreaId,
            PreparationAreaName = e.PreparationArea?.Name ?? "",
            IsActive = e.IsActive,
            IsAvailableToday = e.IsAvailableToday,
            Types = new List<MenuItemTypeViewModel>() // fill separately if needed
        };

        public static Domain.Entities.MenuMod.MenuItem ToDomain(MenuItemViewModel vm) => new()
        {
            Id = vm.Id,
            Name = vm.Name,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
            StockKeepingUnit = vm.StockKeepingUnit,
            TaxRate = vm.TaxRate,
            Barcode = vm.Barcode,
            ImagePath = vm.ImagePath,
            DisplayOrder = vm.DisplayOrder,
            ExpectedPreparationTime = vm.ExpectedPreparationTime,
            PreparationAreaId = vm.PreparationAreaId,
            IsActive = vm.IsActive,
            IsAvailableToday = vm.IsAvailableToday
        };
    }
}
