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
    public class DiningAreaService(AppDbContext context, IIdGeneratorService idGenerator,
        IDateService dateService) : IDiningAreaService
    {
        private readonly AppDbContext _context = context;
        private readonly IIdGeneratorService _idGenerator = idGenerator;
        private readonly IDateService _dateService = dateService;

        // -------- Queries --------
        public async Task<List<DiningArea>> GetAllAsync()
        {
            return await _context.DiningAreas
                .Where(a => !a.IsDeleted)
                .OrderBy(a => a.SortOrder)
                .ToListAsync();
        }

        public async Task<List<DiningArea>> GetActiveAsync()
        {
            return await _context.DiningAreas
                .Where(a => !a.IsDeleted && a.IsActive)
                .OrderBy(a => a.SortOrder)
                .ToListAsync();
        }

        public async Task<DiningArea> GetByIdAsync(long id)
        {
            var area = await _context.DiningAreas
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

            return area ?? throw new ApplicationException("Dining area not found.");
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var normalized = (name ?? "").Trim().ToLower();
            return await _context.DiningAreas.AnyAsync(a =>
                !a.IsDeleted &&
                a.Name.ToLower() == normalized);
        }

        // -------- Commands --------
        public async Task<long> AddAsync(DiningArea area)
        {
            await ValidateAsync(area, false);

            area.Id = await _idGenerator.GenerateNextId<DiningArea>();
            area.CreatedAt = _dateService.Now;

            _context.DiningAreas.Add(area);
            await _context.SaveChangesAsync();
            return area.Id;
        }

        public async Task UpdateAsync(DiningArea area)
        {
            var existing = await GetByIdAsync(area.Id);
            await ValidateAsync(area, true);

            existing.Name = area.Name;
            existing.Description = area.Description ?? "";
            existing.IsReservable = area.IsReservable;
            existing.SortOrder = area.SortOrder;
            existing.IsActive = area.IsActive;
            existing.ModifiedAt = _dateService.Now;

            _context.DiningAreas.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);

            // Optional: prevent delete if area has tables assigned
            var hasTables = await _context.Tables.AnyAsync(t => t.AreaId == id && !t.IsDeleted);
            if (hasTables)
                throw new ApplicationException("Cannot delete: area has tables assigned.");

            existing.IsDeleted = true;
            existing.ModifiedAt = _dateService.Now;

            _context.DiningAreas.Update(existing);
            await _context.SaveChangesAsync();
        }

        // -------- Toggles --------
        public async Task SetActiveAsync(long id, bool isActive)
        {
            var existing = await GetByIdAsync(id);
            existing.IsActive = isActive;
            existing.ModifiedAt = _dateService.Now;
            _context.DiningAreas.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task SetReservableAsync(long id, bool isReservable)
        {
            var existing = await GetByIdAsync(id);
            existing.IsReservable = isReservable;
            existing.ModifiedAt = _dateService.Now;
            _context.DiningAreas.Update(existing);
            await _context.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(DiningArea area, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(area.Name))
                throw new ApplicationException("Dining area name cannot be empty.");

            if (area.SortOrder < 0)
                throw new ApplicationException("Sort order cannot be negative.");

            var exists = await ExistsByNameAsync(area.Name);
            if (exists)
                throw new ApplicationException($"Dining area with name '{area.Name}' already exists.");
        }
    }
}
