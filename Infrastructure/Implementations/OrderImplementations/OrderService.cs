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
    public class OrderService(AppDbContext ctx, IIdGeneratorService id, IDateService date) : IOrderService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // ---------- Queries ----------
        public async Task<Order> GetByIdAsync(long id) =>
            await _ctx.Orders.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted)
            ?? throw new ApplicationException("Order not found.");

        public async Task<Order> GetByNumberAsync(string orderNumber) =>
            await _ctx.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber && !o.IsDeleted)
            ?? throw new ApplicationException("Order not found.");

        public async Task<List<Order>> GetByDateAsync(DateOnly date) =>
        await _ctx.Orders.Where(o => !o.IsDeleted && o.OrderDate == date).ToListAsync();

        public async Task<List<Order>> GetByCustomerAsync(long customerId) =>
        await _ctx.Orders.Where(o => !o.IsDeleted && o.CustomerId == customerId).ToListAsync();

        public async Task<List<Order>> GetByTableAsync(long tableId) =>
        await _ctx.Orders.Where(o => !o.IsDeleted && o.TableId == tableId && !o.IsVoided).ToListAsync();

        public async Task<List<Order>> GetDraftOrdersAsync() =>
            await _ctx.Orders.Where(o => !o.IsDeleted && o.OrderStatus == OrderStatusEnum.Draft).ToListAsync();

        public async Task<List<Order>> GetActiveOrdersAsync() =>
        await _ctx.Orders.Where(o => !o.IsDeleted && !o.IsVoided && o.OrderStatus != OrderStatusEnum.Cancelled).ToListAsync();

        public async Task<bool> ExistsAsync(string orderNumber) =>
            await _ctx.Orders.AnyAsync(o => !o.IsDeleted && o.OrderNumber == orderNumber);
    }
}
