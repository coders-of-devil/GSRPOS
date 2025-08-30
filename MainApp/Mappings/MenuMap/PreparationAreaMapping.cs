using Domain.Entities.MenuMod;
using MainApp.ViewModels.MenuModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Mappings.MenuMap
{
    public class PreparationAreaMapping
    {
        public static PreparationAreaViewModel ToViewModel(PreparationArea entity)
        {
            if (entity is null) return null!;

            return new PreparationAreaViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Remarks = entity.Remarks,
                CreatedAt = entity.CreatedAt
            };
        }

        public static PreparationArea ToDomainModel(PreparationAreaViewModel vm)
        {
            if (vm is null) return null!;

            return new PreparationArea
            {
                Id = vm.Id,
                Name = vm.Name,
                Remarks = vm.Remarks
            };
        }
    }
}
