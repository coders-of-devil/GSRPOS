using Domain.Entities.TableMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.TableInterfaces
{
    public interface ISeatService
    {
        // -------- Queries --------
        Task<Seat> GetByIdAsync(long id);
        Task<List<Seat>> GetAllAsync();
        Task<List<Seat>> GetByTableAsync(long tableId);
        Task<List<Seat>> GetAvailableByTableAsync(long tableId); // not occupied

        // -------- Existence / helpers --------
        Task<bool> ExistsByNameInTableAsync(string name, long tableId);

        // -------- Commands --------
        Task<long> AddAsync(Seat seat);
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(long id); // soft delete

        // -------- Toggles / state --------
        Task SetOccupiedAsync(long id, bool isOccupied);

        // -------- Guards --------
        Task EnsureSeatsWithinCapacityAsync(long tableId, bool autoFixToCapacity = false);
    }
}
