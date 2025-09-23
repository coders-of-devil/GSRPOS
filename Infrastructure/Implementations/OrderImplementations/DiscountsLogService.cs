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
    public class DiscountsLogService(AppDbContext ctx, IIdGeneratorService id, 
        IDateService date) : IDiscountsLogService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<DiscountsLog> GetByIdAsync(long id) =>
            await _ctx.DiscountsLogs.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Discount log not found.");

        public async Task<List<DiscountsLog>> GetByOrderAsync(long orderId) =>
            await _ctx.DiscountsLogs.Where(x => !x.IsDeleted && x.OrderId == orderId).ToListAsync();

        public async Task<List<DiscountsLog>> GetByOrderItemAsync(long orderItemId) =>
            await _ctx.DiscountsLogs.Where(x => !x.IsDeleted && x.OrderItemId == orderItemId).ToListAsync();

        public async Task<List<DiscountsLog>> GetByCouponAsync(string couponCode) =>
            await _ctx.DiscountsLogs.Where(x => !x.IsDeleted && x.DiscountCoupon == couponCode).ToListAsync();

        // -------- Commands --------
        public async Task<long> AddAsync(DiscountsLog log)
        {
            log.Id = await _id.GenerateNextId<DiscountsLog>();
            log.CreatedAt = _date.Now;
            log.AppliedAt = _date.Now;
            _ctx.DiscountsLogs.Add(log);
            await _ctx.SaveChangesAsync();
            return log.Id;
        }

        public async Task UpdateAsync(DiscountsLog log)
        {
            var existing = await GetByIdAsync(log.Id);

            existing.DiscountAmount = log.DiscountAmount;
            existing.DiscountCoupon = log.DiscountCoupon ?? "";
            existing.CouponSource = log.CouponSource ?? "";
            existing.IsCouponBased = log.IsCouponBased;
            existing.AppliedBy = log.AppliedBy;
            existing.AppliedAt = _date.Now;
            existing.ModifiedAt = _date.Now;

            _ctx.DiscountsLogs.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.DiscountsLogs.Update(existing);
            await _ctx.SaveChangesAsync();
        }
    }
}
