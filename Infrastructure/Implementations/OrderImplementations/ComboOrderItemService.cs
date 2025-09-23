using Applications.Interfaces.OrderInterfaces;
using Domain.Entities.OrderMod;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.OrderImplementations
{
    public class ComboOrderItemService(AppDbContext ctx, IIdGeneratorService id, 
        IDateService date) : IComboOrderItemService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<ComboOrderItem> GetByIdAsync(long id) =>
            await _ctx.ComboOrderItems.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Combo order item not found.");

        public async Task<List<ComboOrderItem>> GetByOrderAsync(long orderId) =>
            await _ctx.ComboOrderItems.Where(x => !x.IsDeleted && x.OrderId == orderId).ToListAsync();

        public async Task<List<ComboOrderItem>> GetByComboAsync(long comboItemId) =>
            await _ctx.ComboOrderItems.Where(x => !x.IsDeleted && x.ComboItemId == comboItemId).ToListAsync();

        // -------- Commands --------
        public async Task<long> AddAsync(ComboOrderItem item)
        {
            await ValidateAsync(item, false);
            item.Id = await _id.GenerateNextId<ComboOrderItem>();
            item.OrderDate = item.OrderDate ?? DateOnly.FromDateTime(_date.Now);
            item.CreatedAt = _date.Now;
            _ctx.ComboOrderItems.Add(item);
            await _ctx.SaveChangesAsync();
            return item.Id;
        }

        public async Task UpdateAsync(ComboOrderItem item)
        {
            var existing = await GetByIdAsync(item.Id);
            await ValidateAsync(item, true);

            existing.ComboItemId = item.ComboItemId;
            existing.MenuItemId = item.MenuItemId;
            existing.Quantity = item.Quantity;
            existing.Notes = item.Notes ?? "";
            existing.IsNoNeed = item.IsNoNeed;
            existing.ModifiedAt = _date.Now;

            _ctx.ComboOrderItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.ComboOrderItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Helpers --------
        public async Task SetNoNeedAsync(long id, bool isNoNeed)
        {
            var existing = await GetByIdAsync(id);
            existing.IsNoNeed = isNoNeed;
            existing.ModifiedAt = _date.Now;
            _ctx.ComboOrderItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(ComboOrderItem i, bool isUpdate)
        {
            if (i.Quantity <= 0) throw new ApplicationException("Quantity must be greater than zero.");

            var orderOk = await _ctx.Orders.AnyAsync(o => o.Id == i.OrderId && !o.IsDeleted);
            if (!orderOk) throw new ApplicationException("Order not found.");

            var comboOk = await _ctx.ComboItems.AnyAsync(c => c.Id == i.ComboItemId && !c.IsDeleted);
            if (!comboOk) throw new ApplicationException("Combo item not found.");

            var menuOk = await _ctx.MenuItems.AnyAsync(m => m.Id == i.MenuItemId && !m.IsDeleted);
            if (!menuOk) throw new ApplicationException("Menu item not found.");
        }
    }
}
