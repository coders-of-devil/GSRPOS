using Domain.Entities.OrderMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.OrderInterfaces
{
    public interface IOrderItemModifierService
    {
        // Queries
        Task<List<OrderItemModifier>> GetByOrderItemAsync(long orderItemId);
        Task<OrderItemModifier> GetByIdAsync(long id);

        // Commands
        Task<long> AddAsync(OrderItemModifier modifier);
        Task UpdateAsync(OrderItemModifier modifier);
        Task DeleteAsync(long id);

        // Bulk
        Task UpsertForOrderItemAsync(long orderItemId, IEnumerable<OrderItemModifier> modifiers);
    }
}
