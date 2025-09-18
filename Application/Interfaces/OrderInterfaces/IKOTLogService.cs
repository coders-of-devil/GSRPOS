using Domain.Entities.OrderMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.OrderInterfaces
{
    public interface IKOTLogService
    {
        // Queries
        Task<KOTLog> GetByIdAsync(long id);
        Task<List<KOTLog>> GetByOrderAsync(long orderId);
        Task<List<KOTLog>> GetByDeviceAsync(long deviceId, DateOnly? date = null);
        Task<List<KOTLog>> GetUnprintedAsync();


        // Status / helpers
        Task MarkPrintedAsync(long id, DateTime printedAt);
        Task RecordErrorAsync(long id, string error);
    }
}
