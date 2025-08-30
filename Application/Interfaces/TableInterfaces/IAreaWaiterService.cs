using Domain.Entities.TableMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.TableInterfaces
{
    public interface IAreaWaiterService
    {
        // Queries
        Task<AreaWaitersEntry> GetByIdAsync(long id);
        Task<List<AreaWaitersEntry>> GetByAreaAsync(long areaId);
        Task<List<AreaWaitersEntry>> GetByWaiterAsync(long waiterId);
        Task<bool> ExistsAsync(long areaId, long waiterId);

        // Commands
        Task<long> AssignAsync(long areaId, long waiterId, string? notes = null, DateTime? assignedOn = null);
        Task UpdateAsync(AreaWaitersEntry entry);
        Task UnassignAsync(long id);                 // soft delete

        // Bulk helpers
        Task<int> AssignManyAsync(long areaId, IEnumerable<long> waiterIds, string? notes = null);
        Task<int> UnassignAllFromAreaAsync(long areaId);
        Task<int> MoveWaiterAsync(long waiterId, long fromAreaId, long toAreaId); // unassign+assign
    }
}
