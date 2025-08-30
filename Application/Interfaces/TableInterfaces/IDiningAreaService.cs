using Domain.Entities.TableMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.TableInterfaces
{
    public interface IDiningAreaService
    {
        // -------- Queries --------
        Task<List<DiningArea>> GetAllAsync();               // All dining areas (active + inactive)
        Task<List<DiningArea>> GetActiveAsync();            // Only active dining areas
        Task<DiningArea> GetByIdAsync(long id);             // By Id
        Task<bool> ExistsByNameAsync(string name);          // Ensure uniqueness of name

        // -------- Commands --------
        Task<long> AddAsync(DiningArea area);               // Add new dining area
        Task UpdateAsync(DiningArea area);                  // Update details
        Task DeleteAsync(long id);                          // Soft delete

        // -------- Toggles --------
        Task SetActiveAsync(long id, bool isActive);        // Enable/disable a dining area
        Task SetReservableAsync(long id, bool isReservable);// Mark dining area reserved
    }
}
