using Domain.Entities.OrderMod;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.OrderInterfaces
{
    public interface IOrderService
    {
        // Queries
        Task<Order> GetByIdAsync(long id);
        Task<Order> GetByNumberAsync(string orderNumber);
        Task<List<Order>> GetByDateAsync(DateOnly date);
        Task<List<Order>> GetByCustomerAsync(long customerId);
        Task<List<Order>> GetByTableAsync(long tableId);
        Task<List<Order>> GetDraftOrdersAsync();
        Task<List<Order>> GetActiveOrdersAsync(); // non-voided, non-cancelled

        // Existence
        Task<bool> ExistsAsync(string orderNumber);

        // Commands
        Task<long> AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(long id); // soft delete / void

        // Status / lifecycle
        Task SetStatusAsync(long id, OrderStatusEnum status);
        //Task MarkAsPaidAsync(long id, PaymentTypeEnum paymentType, decimal amountReceived, decimal changeGiven);
        Task MarkAsInvoicedAsync(long id, bool invoiced = true);
        Task VoidAsync(long id, string reason, long userId);
    }
}
