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
    public class TableService(AppDbContext context, IIdGeneratorService idGenerator,
        IDateService dateService) : IDiningTableService
    {
        private readonly AppDbContext _context = context;
        private readonly IDateService _dateService = dateService;
        private readonly IIdGeneratorService _idGenerator = idGenerator;

        // -------- Queries --------
        public async Task<DiningTable> GetByIdAsync(long id)
        {
            var t = await _context.Tables
                .Include(x => x.Area)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return t ?? throw new ApplicationException("Table not found.");
        }

        public async Task<List<DiningTable>> GetAllAsync()
        {
            return await _context.Tables
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.AreaId).ThenBy(x => x.Name)
                .Include(x => x.Area)
                .ToListAsync();
        }

        public async Task<List<DiningTable>> GetByAreaAsync(long areaId)
        {
            await EnsureAreaExists(areaId);
            return await _context.Tables
                .Where(x => !x.IsDeleted && x.AreaId == areaId)
                .Include(x => x.Area)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<DiningTable>> GetActiveByAreaAsync(long areaId)
        {
            await EnsureAreaExists(areaId);
            return await _context.Tables
                .Where(x => !x.IsDeleted && x.AreaId == areaId && x.IsActive)
                .Include(x => x.Area)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<DiningTable>> GetAvailableForReservationAsync(long areaId)
        {
            await EnsureAreaExists(areaId);
            return await _context.Tables
                .Where(x => !x.IsDeleted && x.AreaId == areaId && x.IsActive && x.IsReservable && !x.IsOccupied)
                .Include(x => x.Area)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        // -------- Existence / helpers --------
        public async Task<bool> ExistsByNameInAreaAsync(string name, long areaId)
        {
            var n = (name ?? string.Empty).Trim().ToLower();
            return await _context.Tables.AnyAsync(x =>
                !x.IsDeleted &&
                x.AreaId == areaId &&
                x.Name.ToLower() == n);
        }

        // -------- Commands --------
        public async Task<long> AddAsync(DiningTable table)
        {
            await ValidateAsync(table, isUpdate: false);

            table.Id = await _idGenerator.GenerateNextId<DiningTable>();
            table.CreatedAt = _dateService.Now;

            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return table.Id;
        }

        public async Task UpdateAsync(DiningTable table)
        {
            var existing = await GetByIdAsync(table.Id);
            await ValidateAsync(table, isUpdate: true);

            existing.AreaId = table.AreaId;
            existing.Name = table.Name.Trim();
            existing.Description = table.Description ?? string.Empty;
            existing.Capacity = table.Capacity;
            existing.IsActive = table.IsActive;
            existing.IsOccupied = table.IsOccupied;
            existing.IsReservable = table.IsReservable;
            existing.ModifiedAt = _dateService.Now;

            _context.Tables.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);

            // Optional: guard if you later add active reservations/ongoing orders:
            // bool hasOpenOrders = await _context.Orders.AnyAsync(o => o.TableId == id && o.Status == Open);
            // if (hasOpenOrders) throw new ApplicationException("Cannot delete: table has open orders.");

            existing.IsDeleted = true;
            existing.ModifiedAt = _dateService.Now;
            _context.Tables.Update(existing);
            await _context.SaveChangesAsync();
        }

        // -------- Toggles / state --------
        public async Task SetActiveAsync(long id, bool isActive)
        {
            var existing = await GetByIdAsync(id);
            existing.IsActive = isActive;
            existing.ModifiedAt = _dateService.Now;
            _context.Tables.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task SetReservableAsync(long id, bool isReservable)
        {
            var existing = await GetByIdAsync(id);
            existing.IsReservable = isReservable;
            existing.ModifiedAt = _dateService.Now;
            _context.Tables.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task SetOccupiedAsync(long id, bool isOccupied)
        {
            var existing = await GetByIdAsync(id);

            // Optional business rule: cannot mark occupied if inactive or not reservable
            if (!existing.IsActive)
                throw new ApplicationException("Cannot occupy an inactive table.");

            existing.IsOccupied = isOccupied;
            existing.ModifiedAt = _dateService.Now;
            _context.Tables.Update(existing);
            await _context.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(DiningTable t, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(t.Name))
                throw new ApplicationException("Table name cannot be empty.");

            if (t.Capacity <= 0)
                throw new ApplicationException("Capacity must be greater than zero.");

            await EnsureAreaExists(t.AreaId);

            if (await ExistsByNameInAreaAsync(t.Name, t.AreaId))
                throw new ApplicationException($"A table named '{t.Name}' already exists in this area.");
        }

        private async Task EnsureAreaExists(long areaId)
        {
            var exists = await _context.DiningAreas.AnyAsync(a => a.Id == areaId && !a.IsDeleted);
            if (!exists) throw new ApplicationException("Dining area not found.");
        }
    }
}
