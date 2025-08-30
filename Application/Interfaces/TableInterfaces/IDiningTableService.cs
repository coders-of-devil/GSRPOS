using Domain.Entities.TableMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.TableInterfaces
{
    public interface IDiningTableService
    {
        // -------- Queries --------
        Task<DiningTable> GetByIdAsync(long id);
        Task<List<DiningTable>> GetAllAsync();
        Task<List<DiningTable>> GetByAreaAsync(long areaId);
        Task<List<DiningTable>> GetActiveByAreaAsync(long areaId);
        Task<List<DiningTable>> GetAvailableForReservationAsync(long areaId); // active + reservable + not occupied

        // -------- Existence / validation helpers --------
        Task<bool> ExistsByNameInAreaAsync(string name, long areaId);

        // -------- Commands --------
        Task<long> AddAsync(DiningTable table);
        Task UpdateAsync(DiningTable table);
        Task DeleteAsync(long id); // soft delete

        // -------- Toggles / state --------
        Task SetActiveAsync(long id, bool isActive);
        Task SetReservableAsync(long id, bool isReservable);
        Task SetOccupiedAsync(long id, bool isOccupied);
        
    }
}
