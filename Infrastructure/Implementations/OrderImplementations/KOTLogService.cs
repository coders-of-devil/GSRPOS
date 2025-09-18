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
    public class KOTLogService(AppDbContext ctx, IIdGeneratorService id, 
        IDateService date) : IKOTLogService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<KOTLog> GetByIdAsync(long id) =>
            await _ctx.kOTLogs.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("KOT log not found.");

        public async Task<List<KOTLog>> GetByOrderAsync(long orderId) =>
            await _ctx.kOTLogs.Where(x => !x.IsDeleted && x.OrderId == orderId).ToListAsync();

        public async Task<List<KOTLog>> GetByDeviceAsync(long deviceId, DateOnly? date = null)
        {
            var q = _ctx.kOTLogs.Where(x => !x.IsDeleted && x.KOTDeviceId == deviceId);
            if (date.HasValue) q = q.Where(x => x.OrderDate == date.Value);
            return await q.ToListAsync();
        }

        public async Task<List<KOTLog>> GetUnprintedAsync() =>
            await _ctx.kOTLogs.Where(x => !x.IsDeleted && !x.IsTicketRaised).ToListAsync();

        // -------- Helpers --------
        public async Task MarkPrintedAsync(long id, DateTime printedAt)
        {
            var log = await GetByIdAsync(id);
            log.IsTicketRaised = true;
            log.PrintedAt = printedAt;
            log.ModifiedAt = _date.Now;
            _ctx.kOTLogs.Update(log);
            await _ctx.SaveChangesAsync();
        }

        public async Task RecordErrorAsync(long id, string error)
        {
            var log = await GetByIdAsync(id);
            log.ErrorsOccured = error;
            log.ModifiedAt = _date.Now;
            _ctx.kOTLogs.Update(log);
            await _ctx.SaveChangesAsync();
        }
    }
}
