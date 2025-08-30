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
    public class SeatService(AppDbContext context, IIdGeneratorService idGenerator,
        IDateService dateService) : ISeatService

    {
        private readonly AppDbContext _context = context;
        private readonly IIdGeneratorService _idGenerator = idGenerator;
        private readonly IDateService _dateService = dateService;

        // -------- Queries --------
        public async Task<Seat> GetByIdAsync(long id)
        {
            var seat = await _context.Seats
                .Include(s => s.Table)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            return seat ?? throw new ApplicationException("Seat not found.");
        }

        public async Task<List<Seat>> GetAllAsync()
        {
            return await _context.Seats
                .Where(s => !s.IsDeleted)
                .Include(s => s.Table)
                .OrderBy(s => s.TableId).ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<Seat>> GetByTableAsync(long tableId)
        {
            await EnsureTableExists(tableId);
            return await _context.Seats
                .Where(s => !s.IsDeleted && s.TableId == tableId)
                .Include(s => s.Table)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<Seat>> GetAvailableByTableAsync(long tableId)
        {
            await EnsureTableExists(tableId);
            return await _context.Seats
                .Where(s => !s.IsDeleted && s.TableId == tableId && !s.IsOccupied)
                .Include(s => s.Table)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        // -------- Existence / helpers --------
        public async Task<bool> ExistsByNameInTableAsync(string name, long tableId)
        {
            var n = (name ?? string.Empty).Trim().ToLower();
            return await _context.Seats.AnyAsync(s =>
                !s.IsDeleted &&
                s.TableId == tableId &&
                s.Name.ToLower() == n);
        }

        // -------- Commands --------
        public async Task<long> AddAsync(Seat seat)
        {
            await ValidateAsync(seat, isUpdate: false);

            seat.Id = await _idGenerator.GenerateNextId<Seat>();
            seat.CreatedAt = _dateService.Now;

            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
            return seat.Id;
        }

        public async Task UpdateAsync(Seat seat)
        {
            var existing = await GetByIdAsync(seat.Id);
            await ValidateAsync(seat, isUpdate: true);

            // If table changed, re-validate name uniqueness in the new table
            if (existing.TableId != seat.TableId)
            {
                await EnsureTableExists(seat.TableId);
                if (await ExistsByNameInTableAsync(seat.Name, seat.TableId))
                    throw new ApplicationException($"A seat named '{seat.Name}' already exists in the destination table.");
            }

            existing.TableId = seat.TableId;
            existing.Name = seat.Name.Trim();
            existing.IsOccupied = seat.IsOccupied;
            existing.ModifiedAt = _dateService.Now;

            _context.Seats.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);

            // Optional: guard if there are live reservations/orders bound to a seat
            // if (await _context.Orders.AnyAsync(o => o.SeatId == id && o.Status == Open)) { ... }

            existing.IsDeleted = true;
            existing.ModifiedAt = _dateService.Now;

            _context.Seats.Update(existing);
            await _context.SaveChangesAsync();
        }

        // -------- Toggles / state --------
        public async Task SetOccupiedAsync(long id, bool isOccupied)
        {
            var existing = await GetByIdAsync(id);
            existing.IsOccupied = isOccupied;
            existing.ModifiedAt = _dateService.Now;

            _context.Seats.Update(existing);
            await _context.SaveChangesAsync();
        }


        // -------- Validation --------
        private async Task ValidateAsync(Seat s, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(s.Name))
                throw new ApplicationException("Seat name cannot be empty.");

            await EnsureTableExists(s.TableId);

            if (await ExistsByNameInTableAsync(s.Name, s.TableId))
                throw new ApplicationException($"A seat named '{s.Name}' already exists for this table.");
        }

        private async Task EnsureTableExists(long tableId)
        {
            var exists = await _context.Tables.AnyAsync(t => t.Id == tableId && !t.IsDeleted);
            if (!exists) throw new ApplicationException("Table not found.");
        }

        // -------- Guards --------
        public async Task EnsureSeatsWithinCapacityAsync(long tableId, bool autoFixToCapacity = false)
        {
            var table = await _context.Tables.FirstOrDefaultAsync(t => t.Id == tableId && !t.IsDeleted)
                ?? throw new ApplicationException("Table not found.");

            var seatCount = await _context.Seats.CountAsync(s => !s.IsDeleted && s.TableId == tableId);

            if (seatCount <= table.Capacity) return;

            if (!autoFixToCapacity)
                throw new ApplicationException($"Seat count ({seatCount}) exceeds table capacity ({table.Capacity}).");

            // Auto-fix: soft delete extra seats (oldest first)
            var surplus = seatCount - table.Capacity;
            var toRemove = await _context.Seats
                .Where(s => !s.IsDeleted && s.TableId == tableId)
                .OrderBy(s => s.CreatedAt)
                .Take(surplus)
                .ToListAsync();

            var now = _dateService.Now;
            foreach (var s in toRemove)
            {
                s.IsDeleted = true;
                s.ModifiedAt = now;
                _context.Seats.Update(s);
            }

            await _context.SaveChangesAsync();
        }
    }
}
