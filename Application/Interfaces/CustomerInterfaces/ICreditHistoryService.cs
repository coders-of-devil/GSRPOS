using Domain.Entities.CustomerMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.CustomerInterfaces
{
    public interface ICustomerCreditService
    {
        // Balance & history
        Task<decimal> GetBalanceAsync(long customerId);
        Task<List<CreditHistory>> GetHistoryAsync(long customerId);

        // Mutations
        Task AddCreditAsync(long customerId, decimal amount, string? reference = null, string? notes = null);
        Task DeductCreditAsync(long customerId, decimal amount, string? reference = null, string? notes = null);
        Task TransferCreditAsync(long fromCustomerId, long toCustomerId, decimal amount, string? notes = null);
    }
}
