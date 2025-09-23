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
    public class OrderPrepStatusService(AppDbContext ctx, IIdGeneratorService id, 
        IDateService date) : IOrderPrepStatusService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<OrderPrepStatus> GetByIdAsync(long id) =>
            await _ctx.OrderPrepStatus.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Order prep status not found.");

        public async Task<List<OrderPrepStatus>> GetByOrderAsync(long orderId) =>
            await _ctx.OrderPrepStatus.Where(x => !x.IsDeleted && x.OrderId == orderId).ToListAsync();

        public async Task<List<OrderPrepStatus>> GetByOrderItemAsync(long orderItemId) =>
            await _ctx.OrderPrepStatus.Where(x => !x.IsDeleted && x.OrderItemId == orderItemId).ToListAsync();

        public async Task<List<OrderPrepStatus>> GetByAreaAsync(long areaId, DateOnly? date = null)
        {
            var q = _ctx.OrderPrepStatus.Where(x => !x.IsDeleted && x.AreaId == areaId);
            if (date.HasValue) q = q.Where(x => x.OrderDate == date.Value);
            return await q.ToListAsync();
        }

        // -------- Commands --------
        public async Task<long> AddAsync(OrderPrepStatus status)
        {
            await ValidateAsync(status, false);
            status.Id = await _id.GenerateNextId<OrderPrepStatus>();
            status.OrderDate = status.OrderDate ?? DateOnly.FromDateTime(_date.Now);
            status.CreatedAt = _date.Now;
            _ctx.OrderPrepStatus.Add(status);
            await _ctx.SaveChangesAsync();
            return status.Id;
        }

        public async Task UpdateAsync(OrderPrepStatus status)
        {
            var existing = await GetByIdAsync(status.Id);
            await ValidateAsync(status, true);

            existing.Status = status.Status;
            existing.AreaId = status.AreaId;
            existing.KOTDeviceId = status.KOTDeviceId;
            existing.UpdatedBy = status.UpdatedBy;
            existing.UpdatedAt = _date.Now;
            existing.ModifiedAt = _date.Now;

            _ctx.OrderPrepStatus.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.OrderPrepStatus.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Helpers --------
        public async Task SetStatusAsync(long orderItemId, OrderStatusEnum status, long updatedBy)
        {
            var existing = await _ctx.OrderPrepStatus.FirstOrDefaultAsync(x => !x.IsDeleted && x.OrderItemId == orderItemId);
            if (existing == null)
            {
                existing = new OrderPrepStatus
                {
                    Id = await _id.GenerateNextId<OrderPrepStatus>(),
                    OrderItemId = orderItemId,
                    Status = status,
                    UpdatedBy = updatedBy,
                    UpdatedAt = _date.Now,
                    CreatedAt = _date.Now
                };
                _ctx.OrderPrepStatus.Add(existing);
            }
            else
            {
                existing.Status = status;
                existing.UpdatedBy = updatedBy;
                existing.UpdatedAt = _date.Now;
                existing.ModifiedAt = _date.Now;
                _ctx.OrderPrepStatus.Update(existing);
            }
            await _ctx.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(OrderPrepStatus s, bool isUpdate)
        {
            var orderOk = await _ctx.Orders.AnyAsync(o => o.Id == s.OrderId && !o.IsDeleted);
            if (!orderOk) throw new ApplicationException("Order not found.");

            var itemOk = await _ctx.OrderItems.AnyAsync(i => i.Id == s.OrderItemId && !i.IsDeleted);
            if (!itemOk) throw new ApplicationException("Order item not found.");

            if (s.AreaId.HasValue)
            {
                var areaOk = await _ctx.PreparationAreas.AnyAsync(a => a.Id == s.AreaId && !a.IsDeleted);
                if (!areaOk) throw new ApplicationException("Preparation area not found.");
            }
        }
    }
}
