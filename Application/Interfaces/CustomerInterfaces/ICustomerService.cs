using Domain.Entities.CustomerMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.CustomerInterfaces
{
    public interface ICustomerService
    {
        // -------- Queries --------
        Task<Customer> GetByIdAsync(long id);
        Task<List<Customer>> GetAllAsync();                           // active + inactive unless soft-deleted
        Task<Customer?> GetByPhoneAsync(string phone);
        Task<Customer?> GetByEmailAsync(string email);
        Task<List<Customer>> GetByMembershipAsync(long membershipId);
        Task<List<Customer>> GetBirthdaysBetweenAsync(DateOnly from, DateOnly to);

        // -------- Existence / helpers --------
        Task<bool> ExistsByPhoneAsync(string phone, long? excludeId = null);
        Task<bool> ExistsByEmailAsync(string email, long? excludeId = null);

        // -------- Commands --------
        Task<long> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(long id);                                    // soft delete

        // -------- Membership --------
        Task SetMembershipAsync(long id, long? membershipId);         // null to clear
    }
}
