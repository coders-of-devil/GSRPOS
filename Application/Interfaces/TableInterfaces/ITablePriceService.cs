using Domain.Entities.TableMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.TableInterfaces
{
    public interface ITablePriceService
    {
        // -------- Queries --------
        Task<TablePrice> GetByIdAsync(long id);
        Task<List<TablePrice>> GetByTableAsync(long tableId);                       // all (active+inactive)
        Task<List<TablePrice>> GetActiveByTableAndDayAsync(long tableId, string dayType);
        Task<decimal> GetEffectivePriceAsync(long tableId, DateTime atLocalTime);  // resolves by day+time

        // -------- Validations / helpers --------
        Task<bool> HasOverlapAsync(long tableId, string dayType, TimeOnly from, TimeOnly to);

        // -------- Commands --------
        Task<long> AddAsync(TablePrice price);
        Task UpdateAsync(TablePrice price);
        Task DeleteAsync(long id); // soft delete
        Task SetActiveAsync(long id, bool isActive);
    }
}
