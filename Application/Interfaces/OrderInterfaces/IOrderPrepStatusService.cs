using Domain.Entities.OrderMod;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.OrderInterfaces
{
    public interface IOrderPrepStatusService
    {
        // Queries
        Task<OrderPrepStatus> GetByIdAsync(long id);
        Task<List<OrderPrepStatus>> GetByOrderAsync(long orderId);
        Task<List<OrderPrepStatus>> GetByOrderItemAsync(long orderItemId);
        Task<List<OrderPrepStatus>> GetByAreaAsync(long areaId, DateOnly? date = null);

        // Commands
        Task<long> AddAsync(OrderPrepStatus status);
        Task UpdateAsync(OrderPrepStatus status);
        Task DeleteAsync(long id);

        // Helpers
        Task SetStatusAsync(long orderItemId, OrderStatusEnum status, long updatedBy);
    }
}
