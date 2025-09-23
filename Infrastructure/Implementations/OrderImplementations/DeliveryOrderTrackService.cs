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
    public class DeliveryOrderTrackService(AppDbContext ctx, IIdGeneratorService id, 
        IDateService date) : IDeliveryOrderTrackService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<DeliveryOrderTrack> GetByIdAsync(long id) =>
            await _ctx.DeliveryOrderTrack.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Delivery record not found.");

        public async Task<List<DeliveryOrderTrack>> GetByOrderAsync(long orderId) =>
            await _ctx.DeliveryOrderTrack.Where(x => !x.IsDeleted && x.OrderId == orderId).ToListAsync();

        public async Task<List<DeliveryOrderTrack>> GetAssignedAsync() =>
            await _ctx.DeliveryOrderTrack.Where(x => !x.IsDeleted && x.IsAssigned && !x.IsOrderVoided).ToListAsync();

        public async Task<List<DeliveryOrderTrack>> GetByDeliveryBoyAsync(long deliveryBoyId, DateOnly? date = null)
        {
            var q = _ctx.DeliveryOrderTrack.Where(x => !x.IsDeleted && x.DeliveryBoyId == deliveryBoyId);
            if (date.HasValue) q = q.Where(x => x.OrderDate == date.Value);
            return await q.ToListAsync();
        }

        public async Task<List<DeliveryOrderTrack>> GetByPartnerAsync(long deliveryPartnerId, DateOnly? date = null)
        {
            var q = _ctx.DeliveryOrderTrack.Where(x => !x.IsDeleted && x.DeliveryPartnerId == deliveryPartnerId);
            if (date.HasValue) q = q.Where(x => x.OrderDate == date.Value);
            return await q.ToListAsync();
        }

        // -------- Commands --------
        public async Task<long> AddAsync(DeliveryOrderTrack entity)
        {
            await ValidateAsync(entity, false);
            entity.Id = await _id.GenerateNextId<DeliveryOrderTrack>();
            entity.OrderDate = entity.OrderDate ?? DateOnly.FromDateTime(_date.Now);
            entity.CreatedAt = _date.Now;
            _ctx.DeliveryOrderTrack.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(DeliveryOrderTrack entity)
        {
            var existing = await GetByIdAsync(entity.Id);
            await ValidateAsync(entity, true);

            existing.Status = entity.Status ?? "";
            existing.Remarks = entity.Remarks ?? "";
            existing.IsOwnDelivery = entity.IsOwnDelivery;
            existing.DeliveryBoyId = entity.DeliveryBoyId;
            existing.IsPartnerDelivery = entity.IsPartnerDelivery;
            existing.DeliveryPartnerId = entity.DeliveryPartnerId;
            existing.IsAssigned = entity.IsAssigned;
            existing.AssignedAt = entity.AssignedAt;
            existing.IsPicked = entity.IsPicked;
            existing.ModifiedAt = _date.Now;

            _ctx.DeliveryOrderTrack.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.DeliveryOrderTrack.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Status helpers --------
        public async Task AssignAsync(long id, long deliveryBoyId, bool isOwnDelivery, string remarks = "")
        {
            var existing = await GetByIdAsync(id);

            var userOk = await _ctx.Users.AnyAsync(u => u.Id == deliveryBoyId && !u.IsDeleted);
            if (!userOk) throw new ApplicationException("Delivery boy not found.");

            existing.DeliveryBoyId = deliveryBoyId;
            existing.IsOwnDelivery = isOwnDelivery;
            existing.IsAssigned = true;
            existing.AssignedAt = _date.Now;
            existing.Remarks = remarks ?? "";
            existing.Status = "Assigned";
            existing.ModifiedAt = _date.Now;

            _ctx.DeliveryOrderTrack.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task AssignToPartnerAsync(long id, long partnerId, string remarks = "")
        {
            var existing = await GetByIdAsync(id);

            var partnerOk = await _ctx.DeliveryPlatforms.AnyAsync(p => p.Id == partnerId && !p.IsDeleted);
            if (!partnerOk) throw new ApplicationException("Delivery partner not found.");

            existing.DeliveryPartnerId = partnerId;
            existing.IsPartnerDelivery = true;
            existing.IsAssigned = true;
            existing.AssignedAt = _date.Now;
            existing.Remarks = remarks ?? "";
            existing.Status = "AssignedToPartner";
            existing.ModifiedAt = _date.Now;

            _ctx.DeliveryOrderTrack.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task MarkPickedAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            if (!existing.IsAssigned) throw new ApplicationException("Order must be assigned before pickup.");

            existing.IsPicked = true;
            existing.Status = "Picked";
            existing.ModifiedAt = _date.Now;

            _ctx.DeliveryOrderTrack.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task MarkDeliveredAsync(long id, string remarks = "")
        {
            var existing = await GetByIdAsync(id);
            if (!existing.IsPicked) throw new ApplicationException("Order must be picked before delivery.");

            existing.Status = "Delivered";
            existing.Remarks = remarks ?? "";
            existing.ModifiedAt = _date.Now;

            _ctx.DeliveryOrderTrack.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task VoidDeliveryAsync(long id, string remarks = "")
        {
            var existing = await GetByIdAsync(id);
            existing.IsOrderVoided = true;
            existing.Status = "Voided";
            existing.Remarks = remarks ?? "";
            existing.ModifiedAt = _date.Now;

            _ctx.DeliveryOrderTrack.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(DeliveryOrderTrack d, bool isUpdate)
        {
            var orderOk = await _ctx.Orders.AnyAsync(o => o.Id == d.OrderId && !o.IsDeleted);
            if (!orderOk) throw new ApplicationException("Order not found.");
        }
    }
}
