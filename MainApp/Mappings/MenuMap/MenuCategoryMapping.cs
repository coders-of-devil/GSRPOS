using Domain.Entities.MenuMod;
using MainApp.ViewModels.MenuModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.MenuMap
{
    public class MenuCategoryMapping
    {
        public static MenuCategoryViewModel ToViewModel(MenuCategory category)
        {
            return new MenuCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsParentCategory = category.IsParentCategory,
                ParentCategoryId = category.ParentCategoryId ?? 0,
                ParentCategoryName = category.ParentCategory?.Name ?? "",
                DisplayOrder = category.DisplayOrder,
                IconPath = category.IconPath
            };
        }

        public static MenuCategory ToDomainModel(MenuCategoryViewModel vm)
        {
            return new MenuCategory
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                IsParentCategory = vm.IsParentCategory,
                ParentCategoryId = vm.ParentCategoryId != 0 ? vm.ParentCategoryId : null,
                DisplayOrder = vm.DisplayOrder,
                IconPath = vm.IconPath
            };
        }
    }
}
