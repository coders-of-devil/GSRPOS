using Applications.Interfaces.OrderInterfaces;
using Domain.Entities.OrderMod;
using Domain.Enums;
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
    public class OrderItemService(AppDbContext ctx, IIdGeneratorService id, 
        IDateService date) : IOrderItemService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // ---------- Queries ----------
        public async Task<OrderItem> GetByIdAsync(long id) =>
            await _ctx.OrderItems.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Order item not found.");

        public async Task<List<OrderItem>> GetByOrderAsync(long orderId) =>
            await _ctx.OrderItems.Where(x => !x.IsDeleted && x.OrderId == orderId).ToListAsync();

        public async Task<List<OrderItem>> GetPendingByOrderAsync(long orderId) =>
            await _ctx.OrderItems.Where(x => !x.IsDeleted && x.OrderId == orderId && x.ItemStatus == ItemStatusEnum.Pending).ToListAsync();

        public async Task<List<OrderItem>> GetByMenuItemAsync(long menuItemId, DateOnly? date = null)
        {
            var q = _ctx.OrderItems.Where(x => !x.IsDeleted && x.MenuItemId == menuItemId);
            if (date.HasValue)
                q = q.Where(x => x.OrderDate == date.Value);
            return await q.ToListAsync();
        }

        // ---------- Commands ----------
        public async Task<long> AddAsync(OrderItem item)
        {
            await ValidateAsync(item, false);
            item.Id = await _id.GenerateNextId<OrderItem>();
            item.OrderDate = item.OrderDate ?? DateOnly.FromDateTime(_date.Now);
            item.CreatedAt = _date.Now;
            _ctx.OrderItems.Add(item);
            await _ctx.SaveChangesAsync();
            return item.Id;
        }

        public async Task UpdateAsync(OrderItem item)
        {
            var existing = await GetByIdAsync(item.Id);
            await ValidateAsync(item, true);

            existing.MenuItemId = item.MenuItemId;
            existing.ItemTypeId = item.ItemTypeId;
            existing.Quantity = item.Quantity;
            existing.UnitPrice = item.UnitPrice;
            existing.DiscountAmount = item.DiscountAmount;
            existing.TotalPrice = item.TotalPrice;
            existing.KitchenNotes = item.KitchenNotes ?? "";
            existing.IsKOTSent = item.IsKOTSent;
            existing.IsFOC = item.IsFOC;
            existing.FOCReason = item.FOCReason ?? "";
            existing.ItemStatus = item.ItemStatus;
            existing.ModifiedAt = _date.Now;

            _ctx.OrderItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.OrderItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // ---------- Status ----------
        public async Task SetStatusAsync(long id, ItemStatusEnum status)
        {
            var existing = await GetByIdAsync(id);
            existing.ItemStatus = status;
            existing.ModifiedAt = _date.Now;
            _ctx.OrderItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task MarkAsKOTSentAsync(long id, long deviceId, bool sent = true)
        {
            var existing = await GetByIdAsync(id);
            existing.IsKOTSent = sent;
            existing.ModifiedAt = _date.Now;
            _ctx.OrderItems.Update(existing);

            var log = new KOTLog
            {
                Id = await _id.GenerateNextId<KOTLog>(),
                OrderId = existing.OrderId,
                OrderDate = existing.OrderDate,
                OrderItemId = existing.MenuItemId,
                KOTDeviceId = deviceId,
                IsTicketRaised = false
            };
            _ctx.kOTLogs.Add(log);

            await _ctx.SaveChangesAsync();
        }

        public async Task VoidAsync(long id, string reason, long userId)
        {
            var existing = await GetByIdAsync(id);
            existing.IsVoided = true;
            existing.ModifiedAt = _date.Now;
            _ctx.OrderItems.Update(existing);

            var log = new VoidOrderItemLog
            {
                Id = await _id.GenerateNextId<VoidOrderItemLog>(),
                IsWholeOrder = false,
                OrderId = existing.OrderId,
                IsOnlyItem = true,
                OrderItemId = id,
                Reason = reason,
                VoidedBy = userId,
                VoidedAt = _date.Now,
                CreatedAt = _date.Now
            };
            _ctx.VoidOrderItemLogs.Add(log);

            await _ctx.SaveChangesAsync();
        }

        // ---------- Helpers ----------
        public async Task SetFOCAsync(long id, string reason)
        {
            var existing = await GetByIdAsync(id);
            existing.IsFOC = true;
            existing.FOCReason = reason ?? "";
            existing.ModifiedAt = _date.Now;
            _ctx.OrderItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // ---------- Validation ----------
        private async Task ValidateAsync(OrderItem i, bool isUpdate)
        {
            if (i.Quantity <= 0) throw new ApplicationException("Quantity must be greater than zero.");
            if (i.UnitPrice < 0) throw new ApplicationException("Unit price cannot be negative.");
            if (i.TotalPrice < 0) throw new ApplicationException("Total price cannot be negative.");

            var orderOk = await _ctx.Orders.AnyAsync(o => o.Id == i.OrderId && !o.IsDeleted);
            if (!orderOk) throw new ApplicationException("Order not found.");

            var menuOk = await _ctx.MenuItems.AnyAsync(m => m.Id == i.MenuItemId && !m.IsDeleted);
            if (!menuOk) throw new ApplicationException("Menu item not found.");
        }
    }
}
