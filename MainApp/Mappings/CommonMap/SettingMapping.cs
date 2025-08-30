using Domain.Entities.Common;
using MainApp.ViewModels.CommonModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.CommonMap
{
    public class SettingMapping
    {
        public static SettingsViewModel ToVm(Setting s) => new()
        {
            Id = s.Id,
            IsTaxable = s.IsTaxable,
            IsTaxInclusive = s.IsTaxInclusive,
            IsTaxExclusive = s.IsTaxExclusive,
            TaxRate = s.TaxRate,
            IsMultiplePriceForItem = s.IsMultiplePriceForItem
        };

        public static Setting ToEntity(SettingsViewModel vm) => new()
        {
            Id = vm.Id,
            IsTaxable = vm.IsTaxable,
            IsTaxInclusive = vm.IsTaxInclusive,
            IsTaxExclusive = vm.IsTaxExclusive,
            TaxRate = vm.TaxRate,
            IsMultiplePriceForItem = vm.IsMultiplePriceForItem
        };
    }
}
