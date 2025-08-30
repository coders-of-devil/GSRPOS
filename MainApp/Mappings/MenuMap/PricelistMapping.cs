using Domain.Entities.MenuMod;
using MainApp.ViewModels.MenuModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.MenuMap
{
    public class PricelistMapping
    {
        public static PricelistTypeViewModel ToTypeVm(PricelistTypes t) => new()
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description ?? ""
        };
        public static PricelistTypes ToTypeEntity(PricelistTypeViewModel vm) => new()
        {
            Id = vm.Id,
            Name = vm.Name,
            Description = vm.Description ?? ""
        };

        public static PricelistViewModel ToListVm(PriceList p) => new()
        {
            Id = p.Id,
            TypeId = p.TypeId,
            TypeName = p.PricelistTypes?.Name ?? "",
            Name = p.Name ?? "",
            IsActive = p.IsActive,
            IsTemporary = p.IsTemporary,
            EffectiveFrom = p.EffectiveFrom,
            EffectiveTo = p.EffectiveTo,
            Notes = p.Notes ?? ""
        };
        public static PriceList ToListEntity(PricelistViewModel vm) => new()
        {
            Id = vm.Id,
            TypeId = vm.TypeId,
            Name = vm.Name,
            IsActive = vm.IsActive,
            IsTemporary = vm.IsTemporary,
            EffectiveFrom = vm.EffectiveFrom,
            EffectiveTo = vm.EffectiveTo,
            Notes = vm.Notes
        };
    }
}
