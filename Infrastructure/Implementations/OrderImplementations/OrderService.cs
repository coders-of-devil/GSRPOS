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

        // ---------- Commands ----------
        public async Task<long> AddAsync(Order order)
        {
            await ValidateAsync(order, false);
            order.Id = await _id.GenerateNextId<Order>();
            order.OrderNumber = $"ORD-{order.Id}";
            order.OrderDate = order.OrderDate ?? DateOnly.FromDateTime(_date.Now);
            order.CreatedAt = _date.Now;
            _ctx.Orders.Add(order);
            await _ctx.SaveChangesAsync();
            return order.Id;
        }

        public async Task UpdateAsync(Order order)
        {
            var existing = await GetByIdAsync(order.Id);
            await ValidateAsync(order, true);

            existing.CustomerId = order.CustomerId;
            existing.TableId = order.TableId;
            existing.SeatId = order.SeatId;
            existing.WaiterId = order.WaiterId;
            existing.OrderSource = order.OrderSource;
            existing.OrderStatus = order.OrderStatus;
            existing.DiscountCode = order.DiscountCode ?? "";
            existing.CouponId = order.CouponId;
            existing.DiscountAmount = order.DiscountAmount;
            existing.TaxAmount = order.TaxAmount;
            existing.ServiceCharge = order.ServiceCharge;
            existing.PackingCharge = order.PackingCharge;
            existing.DeliveryCharge = order.DeliveryCharge;
            existing.RoundOfAmount = order.RoundOfAmount;
            existing.GrossTotal = order.GrossTotal;
            existing.NetTotal = order.NetTotal;
            existing.PaidAmount = order.PaidAmount;
            existing.DueAmount = order.DueAmount;
            existing.PaymentStatus = order.PaymentStatus;
            existing.PaymentType = order.PaymentType;
            existing.ReceivedAmount = order.ReceivedAmount;
            existing.ChangeGiven = order.ChangeGiven;
            existing.DeliveryPerson = order.DeliveryPerson ?? "";
            existing.DeliveryDate = order.DeliveryDate;
            existing.Remarks = order.Remarks ?? "";
            existing.NoOfItems = order.NoOfItems;
            existing.IsBillPrinted = order.IsBillPrinted;
            existing.IsInvoiced = order.IsInvoiced;
            existing.ModifiedAt = _date.Now;

            _ctx.Orders.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.Orders.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // ---------- Status / Lifecycle ----------
        public async Task SetStatusAsync(long id, OrderStatusEnum status)
        {
            var order = await GetByIdAsync(id);
            order.OrderStatus = status;
            order.ModifiedAt = _date.Now;
            _ctx.Orders.Update(order);
            await _ctx.SaveChangesAsync();
        }

        public async Task MarkAsInvoicedAsync(long id, bool invoiced = true)
        {
            var order = await GetByIdAsync(id);
            order.IsInvoiced = invoiced;
            order.ModifiedAt = _date.Now;
            _ctx.Orders.Update(order);
            await _ctx.SaveChangesAsync();
        }

        public async Task VoidAsync(long id, string reason, long userId)
        {
            var order = await GetByIdAsync(id);
            order.IsVoided = true;
            order.ModifiedAt = _date.Now;
            _ctx.Orders.Update(order);

            var log = new VoidOrderItemLog
            {
                Id = await _id.GenerateNextId<VoidOrderItemLog>(),
                OrderId = id,
                IsOnlyItem = false,
                IsWholeOrder = true,
                Reason = reason,
                VoidedBy = userId,
                VoidedAt = _date.Now,
                CreatedAt = _date.Now
            };
            _ctx.VoidOrderItemLogs.Add(log);

            await _ctx.SaveChangesAsync();
        }

        // ---------- Validation ----------
        private async Task ValidateAsync(Order o, bool isUpdate)
        {
            if (o.GrossTotal < 0 || o.NetTotal < 0)
                throw new ApplicationException("Totals cannot be negative.");
            if (o.DiscountAmount < 0 || o.TaxAmount < 0)
                throw new ApplicationException("Discount/Tax cannot be negative.");
            if (o.CustomerId.HasValue)
            {
                var custOk = await _ctx.Customers.AnyAsync(c => c.Id == o.CustomerId && !c.IsDeleted);
                if (!custOk) throw new ApplicationException("Customer not found.");
            }
            if (o.TableId.HasValue)
            {
                var tblOk = await _ctx.Tables.AnyAsync(t => t.Id == o.TableId && !t.IsDeleted);
                if (!tblOk) throw new ApplicationException("Table not found.");
            }
        }
    }
}
