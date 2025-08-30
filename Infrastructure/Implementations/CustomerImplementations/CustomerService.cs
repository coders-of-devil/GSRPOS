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
    public class CustomerService(AppDbContext ctx, IIdGeneratorService ids, 
        IDateService date) : ICustomerService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _ids = ids;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<Customer> GetByIdAsync(long id)
        {
            var c = await _ctx.Customers
                .Include(x => x.Membership)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return c ?? throw new ApplicationException("Customer not found.");
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _ctx.Customers
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Customer?> GetByPhoneAsync(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return null;

            return await _ctx.Customers
                .Where(x => !x.IsDeleted && x.Phone == phone)
                .FirstOrDefaultAsync();
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            return await _ctx.Customers
                .Where(x => !x.IsDeleted && x.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Customer>> GetByMembershipAsync(long membershipId)
        {
            return await _ctx.Customers
                .Where(x => !x.IsDeleted && x.MembershipId == membershipId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetBirthdaysBetweenAsync(DateOnly from, DateOnly to)
        {
            return await _ctx.Customers
                .Where(c => !c.IsDeleted && c.DOB != null && c.DOB >= from && c.DOB <= to)
                .OrderBy(c => c.DOB)
                .ToListAsync();
        }

        // -------- Existence / helpers --------
        public async Task<bool> ExistsByPhoneAsync(string phone, long? excludeId = null)
        {
            if (string.IsNullOrEmpty(phone)) return false;

            return await _ctx.Customers.AnyAsync(c =>
                !c.IsDeleted && c.Phone == phone &&
                (!excludeId.HasValue || c.Id != excludeId.Value));
        }

        public async Task<bool> ExistsByEmailAsync(string email, long? excludeId = null)
        {
            if (string.IsNullOrEmpty(email)) return false;

            return await _ctx.Customers.AnyAsync(c =>
                !c.IsDeleted && c.Email == email &&
                (!excludeId.HasValue || c.Id != excludeId.Value));
        }

        // -------- Commands --------
        public async Task<long> AddAsync(Customer customer)
        {
            await ValidateAsync(customer, isUpdate: false);

            customer.Id = await _ids.GenerateNextId<Customer>();
            customer.CreatedAt = _date.Now;

            _ctx.Customers.Add(customer);
            await _ctx.SaveChangesAsync();
            return customer.Id;
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existing = await GetByIdAsync(customer.Id);
            await ValidateAsync(customer, isUpdate: true);

            existing.Name = customer.Name.Trim();
            existing.Phone = customer.Phone ?? string.Empty;
            existing.Email = customer.Email ?? string.Empty;
            existing.DOB = customer.DOB;
            existing.Gender = customer.Gender ?? string.Empty;
            existing.MembershipId = customer.MembershipId;
            existing.LoyaltyPoints = customer.LoyaltyPoints;
            existing.CreditBalance = customer.CreditBalance;
            existing.Source = customer.Source ?? string.Empty;
            existing.Notes = customer.Notes ?? string.Empty;
            existing.ModifiedAt = _date.Now;

            _ctx.Customers.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;

            _ctx.Customers.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Membership --------
        public async Task SetMembershipAsync(long id, long? membershipId)
        {
            var c = await GetByIdAsync(id);

            if (membershipId.HasValue)
            {
                var ok = await _ctx.Memberships.AnyAsync(m => m.Id == membershipId.Value && !m.IsDeleted);
                if (!ok) throw new ApplicationException("Membership not found.");
            }

            c.MembershipId = membershipId;
            c.ModifiedAt = _date.Now;

            _ctx.Customers.Update(c);
            await _ctx.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(Customer c, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(c.Name))
                throw new ApplicationException("Customer name cannot be empty.");

            if (!IsValidEmail(c.Email))
                throw new ApplicationException("Invalid email address.");

            if (c.DOB.HasValue && c.DOB.Value > DateOnly.FromDateTime(_date.Now))
                throw new ApplicationException("DOB cannot be in the future.");

            if (!string.IsNullOrWhiteSpace(c.Phone) && await ExistsByPhoneAsync(c.Phone, isUpdate ? c.Id : null))
                throw new ApplicationException("Phone is already in use.");

            if (!string.IsNullOrWhiteSpace(c.Email) && await ExistsByEmailAsync(c.Email, isUpdate ? c.Id : null))
                throw new ApplicationException("Email is already in use.");
        }

        private static bool IsValidEmail(string email)
        {
            try { var _ = new System.Net.Mail.MailAddress(email); return true; }
            catch { return false; }
        }
    }
}
