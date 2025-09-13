using Domain.Entities.MenuMod;
using MainApp.ViewModels.MenuModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.MenuMap
{
    public static class ModifierMappings
    {
        // ModifierGroup
        public static ModifierGroupViewModel ToGroupViewModel(this ModifierGroup entity) =>
            new ModifierGroupViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                IsRequired = entity.IsRequired,
                MaxSelectable = entity.MaxSelectable,
                DisplayOrder = entity.DisplayOrder,
                Description = entity.Description
            };

        public static ModifierGroup ToGroupEntity(this ModifierGroupViewModel vm) =>
            new ModifierGroup
            {
                Id = vm.Id,
                Name = vm.Name,
                IsRequired = vm.IsRequired,
                MaxSelectable = vm.MaxSelectable,
                DisplayOrder = vm.DisplayOrder,
                Description = vm.Description
            };

        // Modifier
        public static ModifierViewModel ToModifierViewModel(this Modifier entity) =>
            new ModifierViewModel
            {
                Id = entity.Id,
                ModifierGroupId = entity.ModifierGroupId,
                GroupName = entity.ModifierGroup?.Name ?? "",
                Name = entity.Name,
                Price = entity.Price,
                DisplayOrder = entity.DisplayOrder,
                IsDefault = entity.IsDefault
            };

        public static Modifier ToModifierEntity(this ModifierViewModel vm) =>
            new Modifier
            {
                Id = vm.Id,
                ModifierGroupId = vm.ModifierGroupId,
                Name = vm.Name,
                Price = vm.Price,
                DisplayOrder = vm.DisplayOrder,
                IsDefault = vm.IsDefault
            };
    }
}
