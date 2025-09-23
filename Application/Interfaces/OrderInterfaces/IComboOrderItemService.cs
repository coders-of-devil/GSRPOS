using Domain.Entities.OrderMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.OrderInterfaces
{
    public interface IComboOrderItemService
    {
        // Queries
        Task<ComboOrderItem> GetByIdAsync(long id);
        Task<List<ComboOrderItem>> GetByOrderAsync(long orderId);
        Task<List<ComboOrderItem>> GetByComboAsync(long comboItemId);

        // Commands
        Task<long> AddAsync(ComboOrderItem item);
        Task UpdateAsync(ComboOrderItem item);
        Task DeleteAsync(long id);

        // Helpers
        Task SetNoNeedAsync(long id, bool isNoNeed);
    }
}
