using Domain.Entities.OrderMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.OrderInterfaces
{
    public interface IDiscountsLogService
    {
        // Queries
        Task<DiscountsLog> GetByIdAsync(long id);
        Task<List<DiscountsLog>> GetByOrderAsync(long orderId);
        Task<List<DiscountsLog>> GetByOrderItemAsync(long orderItemId);

        // Commands
        Task<long> AddAsync(DiscountsLog log);
        Task UpdateAsync(DiscountsLog log);
        Task DeleteAsync(long id);

        // Helpers
        Task<List<DiscountsLog>> GetByCouponAsync(string couponCode);
    }
}
