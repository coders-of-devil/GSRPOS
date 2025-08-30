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
    public class AreaWaiterService(AppDbContext context, IIdGeneratorService idGenerator,
        IDateService dateService) : IAreaWaiterService
    {
        private readonly AppDbContext _ctx = context;
        private readonly IIdGeneratorService _ids = idGenerator;
        private readonly IDateService _date = dateService;

        // ----- Queries -----
        public async Task<AreaWaitersEntry> GetByIdAsync(long id)
        {
            return await _ctx.AreaWaitersEntries
                .Include(x => x.Area)
                .Include(x => x.Waiter)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Area-waiter assignment not found.");
        }

        public async Task<List<AreaWaitersEntry>> GetByAreaAsync(long areaId)
        {
            return await _ctx.AreaWaitersEntries
                .Where(x => !x.IsDeleted && x.AreaId == areaId)
                .Include(x => x.Waiter)
                .OrderByDescending(x => x.AssignedOn)
                .ToListAsync();
        }

        public async Task<List<AreaWaitersEntry>> GetByWaiterAsync(long waiterId)
        {
            return await _ctx.AreaWaitersEntries
                .Where(x => !x.IsDeleted && x.WaiterId == waiterId)
                .Include(x => x.Area)
                .OrderByDescending(x => x.AssignedOn)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(long areaId, long waiterId)
        {
            return await _ctx.AreaWaitersEntries.AnyAsync(x =>
                !x.IsDeleted &&
                x.AreaId == areaId &&
                x.WaiterId == waiterId);
        }

        // ---- Commands ----
        public async Task<long> AssignAsync(long areaId, long waiterId, string? notes = null, DateTime? assignedOn = null)
        {
            await ValidateAsync(areaId, waiterId);

            var entry = new AreaWaitersEntry
            {
                Id = await _ids.GenerateNextId<AreaWaitersEntry>(),
                AreaId = areaId,
                WaiterId = waiterId,
                AssignedOn = assignedOn ?? _date.Now,
                Notes = notes ?? string.Empty,
                CreatedAt = _date.Now
            };

            _ctx.AreaWaitersEntries.Add(entry);
            await _ctx.SaveChangesAsync();
            return entry.Id;
        }

        public async Task UpdateAsync(AreaWaitersEntry entry)
        {
            var existing = await GetByIdAsync(entry.Id);

            await ValidateAsync(entry.AreaId, entry.WaiterId);

            existing.AreaId = entry.AreaId;
            existing.WaiterId = entry.WaiterId;
            existing.AssignedOn = entry.AssignedOn == default ? existing.AssignedOn : entry.AssignedOn;
            existing.Notes = entry.Notes ?? string.Empty;
            existing.ModifiedAt = _date.Now;

            _ctx.AreaWaitersEntries.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task UnassignAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.AreaWaitersEntries.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // ---- Bulk helpers ----
        public async Task<int> AssignManyAsync(long areaId, IEnumerable<long> waiterIds, string? notes = null)
        {
            var list = waiterIds.Distinct().ToList();
            if (list.Count == 0) return 0;

            var now = _date.Now;

            // Validate area once
            await EnsureAreaExists(areaId);

            // Get current waiter ids to avoid duplicates
            var existing = await _ctx.AreaWaitersEntries
                .Where(x => !x.IsDeleted && x.AreaId == areaId)
                .Select(x => x.WaiterId)
                .ToListAsync();

            int created = 0;
            foreach (var waiterId in list)
            {
                await EnsureWaiterExists(waiterId);

                if (existing.Contains(waiterId)) continue; // skip duplicate
                var e = new AreaWaitersEntry
                {
                    Id = await _ids.GenerateNextId<AreaWaitersEntry>(),
                    AreaId = areaId,
                    WaiterId = waiterId,
                    AssignedOn = now,
                    Notes = notes ?? string.Empty,
                    CreatedAt = now
                };
                _ctx.AreaWaitersEntries.Add(e);
                created++;
            }
            await _ctx.SaveChangesAsync();
            return created;
        }

        public async Task<int> UnassignAllFromAreaAsync(long areaId)
        {
            var rows = await _ctx.AreaWaitersEntries
                .Where(x => !x.IsDeleted && x.AreaId == areaId)
                .ToListAsync();

            var now = _date.Now;
            foreach (var r in rows)
            {
                r.IsDeleted = true;
                r.ModifiedAt = now;
            }
            await _ctx.SaveChangesAsync();
            return rows.Count;
        }

        public async Task<int> MoveWaiterAsync(long waiterId, long fromAreaId, long toAreaId)
        {
            await EnsureAreaExists(fromAreaId);
            await EnsureAreaExists(toAreaId);
            await EnsureWaiterExists(waiterId);

            var existing = await _ctx.AreaWaitersEntries
                .FirstOrDefaultAsync(x => !x.IsDeleted && x.AreaId == fromAreaId && x.WaiterId == waiterId)
                ?? throw new ApplicationException("Assignment not found in source area.");

            // avoid duplicate in destination
            if (await ExistsAsync(toAreaId, waiterId))
                throw new ApplicationException("Waiter is already assigned to the destination area.");

            // soft-delete old assignment, create new
            var now = _date.Now;
            existing.IsDeleted = true;
            existing.ModifiedAt = now;

            var newEntry = new AreaWaitersEntry
            {
                Id = await _ids.GenerateNextId<AreaWaitersEntry>(),
                AreaId = toAreaId,
                WaiterId = waiterId,
                AssignedOn = now,
                Notes = existing.Notes,
                CreatedAt = now
            };

            _ctx.AreaWaitersEntries.Add(newEntry);
            await _ctx.SaveChangesAsync();
            return 1;
        }

        // ---- Validation & helpers ----
        private async Task ValidateAsync(long areaId, long waiterId)
        {
            await EnsureAreaExists(areaId);
            await EnsureWaiterExists(waiterId);

            // Prevent duplicate active assignment
            if (await ExistsAsync(areaId, waiterId))
                throw new ApplicationException("Waiter is already assigned to this area.");
        }

        private async Task EnsureAreaExists(long areaId)
        {
            var ok = await _ctx.DiningAreas.AnyAsync(a => a.Id == areaId && !a.IsDeleted);
            if (!ok) throw new ApplicationException("Dining area not found.");
        }

        private async Task EnsureWaiterExists(long waiterId)
        {
            var ok = await _ctx.Users.AnyAsync(u => u.Id == waiterId && !u.IsDeleted /* && u.Role == Waiter if you track roles */);
            if (!ok) throw new ApplicationException("Waiter not found.");
        }
    }
}
