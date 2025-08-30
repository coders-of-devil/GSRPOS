using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IMenuItemPricingService
    {
        // Queries
        Task<MenuItemPriceListEntry> GetByIdAsync(long id);
        Task<List<MenuItemPriceListEntry>> GetByPriceListAsync(long priceListId);
        Task<List<MenuItemPriceListEntry>> GetByItemAsync(long itemId);

        // Existence / helpers
        Task<bool> ExistsAsync(long priceListId, long itemId, long? excludeId = null);

        // Commands
        Task<long> AddAsync(MenuItemPriceListEntry entry);
        Task UpdateAsync(MenuItemPriceListEntry entry);
        Task DeleteAsync(long id);

        // Optional: price resolver using active, date-effective lists (if you maintain dates on Pricelist)
        Task<decimal> GetEffectiveBasePriceAsync(long itemId, DateTime atUtc);
    }

}
