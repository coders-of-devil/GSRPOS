using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IPreparationAreaService
    {
        Task<List<PreparationArea>> GetAllPreparationAreaAsync();
        Task<PreparationArea> GetPreparationAreaByIdAsync(long id);
        Task CreatePreparationAreaAsync(PreparationArea preparationArea);
        Task UpdatePreparationAreaAsync(PreparationArea preparationArea);
        Task DeletePreparationAreaAsync(long id);
    }
}
