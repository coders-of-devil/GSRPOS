using Domain.Entities.OrderMod;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.OrderInterfaces
{
    public interface IOrderItemService
    {
        // Queries
        Task<OrderItem> GetByIdAsync(long id);
        Task<List<OrderItem>> GetByOrderAsync(long orderId);
        Task<List<OrderItem>> GetPendingByOrderAsync(long orderId);
        Task<List<OrderItem>> GetByMenuItemAsync(long menuItemId, DateOnly? date = null);

        // Commands
        Task<long> AddAsync(OrderItem item);
        Task UpdateAsync(OrderItem item);
        Task DeleteAsync(long id);

        // Status
        Task SetStatusAsync(long id, ItemStatusEnum status);
        Task MarkAsKOTSentAsync(long id, bool sent = true);
        Task VoidAsync(long id, string reason, long userId, long? managerId = null);

        // Helpers
        Task SetFOCAsync(long id, string reason);
    }
}
