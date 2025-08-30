using Applications.Interfaces.TableInterfaces;
using Domain.Entities.TableMod;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.TableImplementations
{
    public class TablePriceService(AppDbContext context, IIdGeneratorService idGenerator,
        IDateService dateService) : ITablePriceService
    {
        private readonly AppDbContext _ctx = context;
        private readonly IIdGeneratorService _ids = idGenerator;
        private readonly IDateService _date = dateService;

        // -------- Queries --------
        public async Task<TablePrice> GetByIdAsync(long id)
        {
            var tp = await _ctx.TablePrices
                .Include(x => x.Table)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return tp ?? throw new ApplicationException("Table price not found.");
        }

        public async Task<List<TablePrice>> GetByTableAsync(long tableId)
        {
            await EnsureTableExists(tableId);
            return await _ctx.TablePrices
                .Where(x => !x.IsDeleted && x.TableId == tableId)
                .Include(x => x.Table)
                .OrderBy(x => x.DayType).ThenBy(x => x.FromTime)
                .ToListAsync();
        }

        public async Task<List<TablePrice>> GetActiveByTableAndDayAsync(long tableId, string dayType)
        {
            await EnsureTableExists(tableId);
            var d = Normalize(dayType);
            return await _ctx.TablePrices
                .Where(x => !x.IsDeleted && x.TableId == tableId && x.IsActive && x.DayType.ToLower() == d)
                .Include(x => x.Table)
                .OrderBy(x => x.FromTime)
                .ToListAsync();
        }

        // Resolve current price by local datetime (matches DayType + time window)
        public async Task<decimal> GetEffectivePriceAsync(long tableId, DateTime atLocalTime)
        {
            await EnsureTableExists(tableId);

            // Map DateTime -> DayType you use.
            // If DayType values are things like "Weekday", "Weekend", or actual day names:
            // Replace this logic accordingly.
            var dayType = MapDateToDayType(atLocalTime);

            var t = TimeOnly.FromDateTime(atLocalTime);
            var d = Normalize(dayType);

            var price = await _ctx.TablePrices
                .Where(x => !x.IsDeleted &&
                            x.TableId == tableId &&
                            x.IsActive &&
                            x.DayType.ToLower() == d &&
                            x.FromTime <= t && t < x.ToTime)       // half-open window [from, to)
                .OrderByDescending(x => x.FromTime)
                .Select(x => x.PricePerHour)
                .FirstOrDefaultAsync();

            if (price <= 0)
                throw new ApplicationException("No effective table price found for the given time.");

            return price;
        }

        // -------- Validations / helpers --------
        public async Task<bool> HasOverlapAsync(long tableId, string dayType, TimeOnly from, TimeOnly to)
        {
            var d = Normalize(dayType);
            // Overlap check for [from, to) with any existing [e.FromTime, e.ToTime)
            return await _ctx.TablePrices.AnyAsync(e =>
                !e.IsDeleted &&
                e.TableId == tableId &&
                e.DayType.ToLower() == d  &&
                e.FromTime < to && from < e.ToTime);
        }

        // -------- Commands --------
        public async Task<long> AddAsync(TablePrice p)
        {
            await ValidateAsync(p, isUpdate: false);

            p.Id = await _ids.GenerateNextId<TablePrice>();
            p.CreatedAt = _date.Now;

            _ctx.TablePrices.Add(p);
            await _ctx.SaveChangesAsync();
            return p.Id;
        }

        public async Task UpdateAsync(TablePrice p)
        {
            var existing = await GetByIdAsync(p.Id);
            await ValidateAsync(p, isUpdate: true);

            existing.TableId = p.TableId;
            existing.DayType = p.DayType?.Trim() ?? string.Empty;
            existing.FromTime = p.FromTime;
            existing.ToTime = p.ToTime;
            existing.PricePerHour = p.PricePerHour;
            existing.IsActive = p.IsActive;
            existing.ModifiedAt = _date.Now;

            _ctx.TablePrices.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.TablePrices.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task SetActiveAsync(long id, bool isActive)
        {
            var existing = await GetByIdAsync(id);
            existing.IsActive = isActive;
            existing.ModifiedAt = _date.Now;
            _ctx.TablePrices.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Internal validation --------
        private async Task ValidateAsync(TablePrice p, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(p.DayType))
                throw new ApplicationException("Day type cannot be empty.");

            if (p.PricePerHour < 0)
                throw new ApplicationException("Price per hour cannot be negative.");

            if (p.FromTime >= p.ToTime)
                throw new ApplicationException("FromTime must be earlier than ToTime.");

            await EnsureTableExists(p.TableId);

            if (await HasOverlapAsync(p.TableId, p.DayType, p.FromTime, p.ToTime))
                throw new ApplicationException("Time range overlaps with an existing price window for this day type.");
        }

        private async Task EnsureTableExists(long tableId)
        {
            var ok = await _ctx.Tables.AnyAsync(t => t.Id == tableId && !t.IsDeleted);
            if (!ok) throw new ApplicationException("Table not found.");
        }

        private static string Normalize(string? s) => (s ?? string.Empty).Trim().ToLower();

        // Adjust this mapping to your DayType convention:
        private static string MapDateToDayType(DateTime localTime)
        {
            // Example 1: Day names ("monday", "tuesday", ...)
            return localTime.DayOfWeek.ToString(); // "Monday"...
                                                   // Example 2: Weekday/Weekend:
                                                   // return (localTime.DayOfWeek == DayOfWeek.Saturday || localTime.DayOfWeek == DayOfWeek.Sunday)
                                                   //     ? "Weekend" : "Weekday";
        }
    }
}
