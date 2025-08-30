using Domain.Entities.CustomerMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.CustomerInterfaces
{
    public interface ICustomerAddressService
    {
        Task<List<CustomerAddress>> GetByCustomerAsync(long customerId);
        Task<CustomerAddress> GetByIdAsync(long id);

        Task<long> AddAsync(CustomerAddress address);                 // address.CustomerId must be set
        Task UpdateAsync(CustomerAddress address);
        Task DeleteAsync(long id);                                    // soft delete

        Task SetDefaultAsync(long customerId, long addressId);        // optional if you track default
    }
}
