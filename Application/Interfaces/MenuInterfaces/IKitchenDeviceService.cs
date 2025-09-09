using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IKitchenDeviceService
    {
        // Queries
        Task<List<KitchenDevice>> GetAllAsync();
        Task<KitchenDevice> GetByIdAsync(long id);
        Task<List<KitchenDevice>> GetByPreparationAreaAsync(long preparationAreaId);
        Task<List<KitchenDevice>> GetActiveAsync();
        Task<List<KitchenDevice>> GetByTypeAsync(string type);

        // Existence checks
        Task<bool> ExistsByNameAsync(string name, long? preparationAreaId, long? excludeId = null);
        Task<bool> ExistsByNetworkAsync(string ipAddress, string port, long? excludeId = null);

        // Commands
        Task<long> AddAsync(KitchenDevice device);
        Task UpdateAsync(KitchenDevice device);
        Task DeleteAsync(long id);

        // Toggles / helpers
        Task SetActiveAsync(long id, bool isActive);
        Task SetDefaultCopiesAsync(long id, int copies);
        Task AssignToAreaAsync(long id, long? preparationAreaId); // pass null to unassign
    }
}
