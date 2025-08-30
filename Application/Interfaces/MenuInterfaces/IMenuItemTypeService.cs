using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IMenuItemTypeService
    {
        // Queries
        Task<List<MenuItemType>> GetByItemAsync(long itemId);
        Task<List<MenuItemType>> GetByItemAndPricelistAsync(long itemId, long pricelistId);
        Task<MenuItemType> GetByIdAsync(long id);

        // Existence / validation helpers
        Task<bool> ExistsAsync(long itemId, long pricelistId, string typeName, long? excludeId = null);

        // Commands
        Task<long> AddAsync(MenuItemType type);
        Task UpdateAsync(MenuItemType type);
        Task DeleteAsync(long id);
    }

}
