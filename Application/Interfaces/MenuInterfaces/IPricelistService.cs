using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IPricelistService
    {
        //--------Pricelist Type--------
        Task<List<PricelistTypes>> GetAllPricelistTypesAsync();
        Task<PricelistTypes> GetPricelistTypeByIdAsync(long id);
        Task AddPricelistTypeAsync(PricelistTypes type);
        Task UpdateTypeAsync(PricelistTypes type);
        Task DeleteTypeAsync(long id);

        //--------Pricelist--------
        Task<List<PriceList>> GetAllPricelistsAsync();
        Task<List<PriceList>> GetPricelistsByType(long typeId);
        Task<PriceList> GetPricelistByIdAsync(long id);
        Task AddPricelistAsync(PriceList pricelist);
        Task UpdatePricelistAsync(PriceList pricelist);
        Task DeletePricelistAsync(long id);

        // -------- Validations / helpers --------
        Task<bool> ExistsTypeByNameAsync(string name);
        Task<bool> ExistsPricelistNameAsync(string name, long typeId);
        Task<bool> HasOverlappingPeriodAsync(long typeId, DateTime from, DateTime to);
        Task SetPricelistActiveAsync(long id, bool isActive);
    }
}
