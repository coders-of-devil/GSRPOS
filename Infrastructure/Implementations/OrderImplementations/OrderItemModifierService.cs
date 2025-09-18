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
    public class OrderItemModifierService(AppDbContext ctx, IIdGeneratorService id, 
        IDateService date) : IOrderItemModifierService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<List<OrderItemModifier>> GetByOrderItemAsync(long orderItemId) =>
            await _ctx.OrderItemModifiers
                .Where(x => !x.IsDeleted && x.OrderItemId == orderItemId)
                .ToListAsync();

        public async Task<OrderItemModifier> GetByIdAsync(long id) =>
            await _ctx.OrderItemModifiers
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Order item modifier not found.");

        // -------- Commands --------
        public async Task<long> AddAsync(OrderItemModifier modifier)
        {
            await ValidateAsync(modifier, false);
            modifier.Id = await _id.GenerateNextId<OrderItemModifier>();
            modifier.CreatedAt = _date.Now;
            _ctx.OrderItemModifiers.Add(modifier);
            await _ctx.SaveChangesAsync();
            return modifier.Id;
        }

        public async Task UpdateAsync(OrderItemModifier modifier)
        {
            var existing = await GetByIdAsync(modifier.Id);
            await ValidateAsync(modifier, true);

            existing.ModifierId = modifier.ModifierId;
            existing.Note = modifier.Note ?? "";
            existing.Price = modifier.Price;
            existing.ModifiedAt = _date.Now;

            _ctx.OrderItemModifiers.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.OrderItemModifiers.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(OrderItemModifier m, bool isUpdate)
        {
            if (m.Price < 0) throw new ApplicationException("Modifier price cannot be negative.");

            var orderOk = await _ctx.Orders.AnyAsync(o => o.Id == m.OrderId && !o.IsDeleted);
            if (!orderOk) throw new ApplicationException("Order not found.");

            var orderItemOk = await _ctx.OrderItems.AnyAsync(i => i.Id == m.OrderItemId && !i.IsDeleted);
            if (!orderItemOk) throw new ApplicationException("Order item not found.");

            var modOk = await _ctx.Modifiers.AnyAsync(md => md.Id == m.ModifierId && !md.IsDeleted);
            if (!modOk) throw new ApplicationException("Modifier not found.");
        }
    }
}
