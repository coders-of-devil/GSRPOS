using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IKitchenOrderRoutingService
    {
        // Queries
        Task<List<KitchenOrderRouting>> GetAllAsync();
        Task<KitchenOrderRouting> GetByIdAsync(long id);
        Task<List<KitchenOrderRouting>> GetByMenuItemAsync(long menuItemId);
        Task<List<KitchenOrderRouting>> GetByKitchenDeviceAsync(long kitchenDeviceId);
        Task<KitchenOrderRouting?> GetRouteAsync(long menuItemId, long kitchenDeviceId);

        // Existence
        Task<bool> ExistsAsync(long menuItemId, long kitchenDeviceId, long? excludeId = null);

        // Commands
        Task<long> AddAsync(KitchenOrderRouting route);
        Task UpdateAsync(KitchenOrderRouting route);
        Task DeleteAsync(long id);

    }

}
