using Domain.Entities.TableMod;
using MainApp.ViewModels.TableModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.TableMap
{
    public static class DiningAreaMappings
    {
        public static DiningAreaViewModel ToViewModel(this DiningArea entity)
        {
            return new DiningAreaViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsReservable = entity.IsReservable,
                SortOrder = entity.SortOrder,
                IsActive = entity.IsActive
            };
        }

        public static DiningArea ToEntity(this DiningAreaViewModel vm)
        {
            return new DiningArea
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                IsReservable = vm.IsReservable,
                SortOrder = vm.SortOrder,
                IsActive = vm.IsActive
            };
        }
    }
}
