using Applications.Interfaces.CustomerInterfaces;
using Domain.Entities.CustomerMod;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.CustomerImplementations
{
    public class CustomerAddressService(AppDbContext ctx, IIdGeneratorService ids,
        IDateService date) : ICustomerAddressService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _ids = ids;
        private readonly IDateService _date = date;

        public async Task<List<CustomerAddress>> GetByCustomerAsync(long customerId)
        {
            await EnsureCustomerExists(customerId);
            return await _ctx.CustomerAddresses
                .Where(a => !a.IsDeleted && a.CustomerId == customerId)
                .OrderByDescending(a => a.IsDefault).ThenBy(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<CustomerAddress> GetByIdAsync(long id)
        {
            var a = await _ctx.CustomerAddresses
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return a ?? throw new ApplicationException("Address not found.");
        }

        public async Task<long> AddAsync(CustomerAddress address)
        {
            await ValidateAsync(address, isUpdate: false);

            address.Id = await _ids.GenerateNextId<CustomerAddress>();
            address.CreatedAt = _date.Now;

            // If making this default, unset others
            if (address.IsDefault)
                await UnsetOtherDefaults(address.CustomerId);

            _ctx.CustomerAddresses.Add(address);
            await _ctx.SaveChangesAsync();
            return address.Id;
        }

        public async Task UpdateAsync(CustomerAddress address)
        {
            var existing = await GetByIdAsync(address.Id);
            await ValidateAsync(address, isUpdate: true);

            existing.CustomerId = address.CustomerId;
            existing.Label = address.Label ?? string.Empty;
            existing.FlatOrOffice = address.FlatOrOffice ?? string.Empty;
            existing.Street = address.Street ?? string.Empty;
            existing.City = address.City ?? string.Empty;
            existing.PostalCode = address.PostalCode ?? string.Empty;
            existing.Country = address.Country ?? string.Empty;
            existing.IsActive = address.IsActive;
            existing.ModifiedAt = _date.Now;

            var defaultChanged = existing.IsDefault != address.IsDefault;
            existing.IsDefault = address.IsDefault;

            if (address.IsDefault && defaultChanged)
                await UnsetOtherDefaults(address.CustomerId);

            _ctx.CustomerAddresses.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;

            _ctx.CustomerAddresses.Update(existing);
            await _ctx.SaveChangesAsync();

            // Optional: if you deleted the default, you may auto-promote the newest remaining address to default
            await EnsureOneDefaultIfAny(existing.CustomerId);
        }

        public async Task SetDefaultAsync(long customerId, long addressId)
        {
            await EnsureCustomerExists(customerId);

            var addr = await _ctx.CustomerAddresses
                .FirstOrDefaultAsync(a => a.Id == addressId && !a.IsDeleted && a.CustomerId == customerId)
                ?? throw new ApplicationException("Address not found for this customer.");

            await UnsetOtherDefaults(customerId);

            addr.IsDefault = true;
            addr.ModifiedAt = _date.Now;

            _ctx.CustomerAddresses.Update(addr);
            await _ctx.SaveChangesAsync();
        }

        // -------- Helpers --------
        private async Task EnsureCustomerExists(long customerId)
        {
            var ok = await _ctx.Customers.AnyAsync(c => c.Id == customerId && !c.IsDeleted);
            if (!ok) throw new ApplicationException("Customer not found.");
        }

        private async Task ValidateAsync(CustomerAddress a, bool isUpdate)
        {
            await EnsureCustomerExists(a.CustomerId);

            // Basic sanity checks (tighten if required)
            if (string.IsNullOrWhiteSpace(a.Label))
                a.Label = "Address";

            if (!a.IsActive && a.IsDefault)
                throw new ApplicationException("Default address must be active.");
        }

        private async Task UnsetOtherDefaults(long customerId)
        {
            var others = await _ctx.CustomerAddresses
                .Where(a => !a.IsDeleted && a.CustomerId == customerId && a.IsDefault)
                .ToListAsync();

            var now = _date.Now;
            foreach (var o in others)
            {
                o.IsDefault = false;
                o.ModifiedAt = now;
                _ctx.CustomerAddresses.Update(o);
            }
            // Intentionally not calling SaveChanges here; caller will SaveChanges after its updates.
        }

        private async Task EnsureOneDefaultIfAny(long customerId)
        {
            var hasDefault = await _ctx.CustomerAddresses
                .AnyAsync(a => !a.IsDeleted && a.CustomerId == customerId && a.IsDefault);

            if (hasDefault) return;

            var candidate = await _ctx.CustomerAddresses
                .Where(a => !a.IsDeleted && a.CustomerId == customerId && a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync();

            if (candidate != null)
            {
                candidate.IsDefault = true;
                candidate.ModifiedAt = _date.Now;
                _ctx.CustomerAddresses.Update(candidate);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
