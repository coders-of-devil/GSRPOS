using Applications.Interfaces.MenuInterfaces;
using Domain.Entities.MenuMod;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.MenuImplementations
{
    public class PricelistService(AppDbContext context, IIdGeneratorService idGenerator,
        IDateService dateService) : IPricelistService
    {
        private readonly IDateService _dateService = dateService;
        private readonly IIdGeneratorService _idGenerator = idGenerator;
        private readonly AppDbContext _context = context;

        // ---------- Types ----------
        public async Task<List<PricelistTypes>> GetAllPricelistTypesAsync()
        {
            return await _context.PricelistTypes
            .Where(t => !t.IsDeleted)
            .OrderBy(t => t.Name)
            .ToListAsync();
        }

        public async Task<PricelistTypes> GetPricelistTypeByIdAsync(long id)
        {
            var t = await _context.PricelistTypes.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return t ?? throw new ApplicationException("Pricelist type not found.");
        }

        public async Task AddPricelistTypeAsync(PricelistTypes type)
        {
            if (string.IsNullOrWhiteSpace(type.Name))
                throw new ApplicationException("Type name cannot be empty.");

            var normalized = type.Name.Trim().ToLower();
            var exists = await _context.PricelistTypes.AnyAsync(x => !x.IsDeleted && x.Name.ToLower() == normalized);
            if (exists) throw new ApplicationException($"Type '{type.Name}' already exists.");

            type.Id = await _idGenerator.GenerateNextId<PricelistTypes>();

            _context.PricelistTypes.Add(type);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTypeAsync(PricelistTypes type)
        {
            var existing = await GetPricelistTypeByIdAsync(type.Id);

            if (string.IsNullOrWhiteSpace(type.Name))
                throw new ApplicationException("Type name cannot be empty.");

            var normalized = type.Name.Trim().ToLower();
            var exists = await _context.PricelistTypes.AnyAsync(x =>
                x.Id != type.Id && !x.IsDeleted && x.Name.ToLower() == normalized);
            if (exists) throw new ApplicationException($"Type '{type.Name}' already exists.");

            existing.Name = type.Name;
            existing.Description = type.Description ?? string.Empty;
            existing.ModifiedAt = _dateService.Now;

            _context.PricelistTypes.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTypeAsync(long id)
        {
            var existing = await GetPricelistTypeByIdAsync(id);

            var hasPricelists = await _context.PriceLists.AnyAsync(p => p.TypeId == id && !p.IsDeleted);
            if (hasPricelists)
                throw new ApplicationException("Cannot delete: there are pricelists under this type.");

            existing.IsDeleted = true;
            existing.ModifiedAt = _dateService.Now;

            _context.PricelistTypes.Update(existing);
            await _context.SaveChangesAsync();
        }

        // ---------- Pricelists ----------
        public async Task<List<PriceList>> GetAllPricelistsAsync()
        {
            return await _context.PriceLists
                .Include(p => p.PricelistTypes)
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.IsActive).ThenBy(p => p.TypeId).ThenBy(p => p.EffectiveFrom)
                .ToListAsync();
        }

        public async Task<List<PriceList>> GetPricelistsByType(long typeId)
        {
            await EnsureTypeExists(typeId);
            return await _context.PriceLists
                .Where(p => p.TypeId == typeId && !p.IsDeleted)
                .OrderByDescending(p => p.IsActive).ThenBy(p => p.EffectiveFrom)
                .ToListAsync();
        }

        public async Task<PriceList> GetPricelistByIdAsync(long id)
        {
            var p = await _context.PriceLists
                .Include(x => x.PricelistTypes)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return p ?? throw new ApplicationException("Pricelist not found.");
        }

        public async Task AddPricelistAsync(PriceList pricelist)
        {
            await ValidatePricelistAsync(pricelist, isUpdate: false);

            pricelist.Id = await _idGenerator.GenerateNextId<PriceList>();

            _context.PriceLists.Add(pricelist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePricelistAsync(PriceList pricelist)
        {
            var existing = await GetPricelistByIdAsync(pricelist.Id);
            await ValidatePricelistAsync(pricelist, isUpdate: true);

            existing.TypeId = pricelist.TypeId;
            existing.Name = pricelist.Name;
            existing.IsActive = pricelist.IsActive;
            existing.IsTemporary = pricelist.IsTemporary;
            existing.EffectiveFrom = pricelist.EffectiveFrom;
            existing.EffectiveTo = pricelist.EffectiveTo;
            existing.Notes = pricelist.Notes ?? string.Empty;
            existing.ModifiedAt = _dateService.Now;

            _context.PriceLists.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePricelistAsync(long id)
        {
            var existing = await GetPricelistByIdAsync(id);

            // If you have child tables (e.g., PriceListItems), either soft-delete them here or block delete.
            existing.IsDeleted = true;
            existing.ModifiedAt = _dateService.Now;

            _context.PriceLists.Update(existing);
            await _context.SaveChangesAsync();
        }

        // ---------- Validations / helpers ----------
        public async Task SetPricelistActiveAsync(long id, bool isActive)
        {
            var existing = await GetPricelistByIdAsync(id);
            existing.IsActive = isActive;
            existing.ModifiedAt = _dateService.Now;

            _context.PriceLists.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsTypeByNameAsync(string name)
        {
            var n = (name ?? "").Trim().ToLower();
            return await _context.PricelistTypes.AnyAsync(x =>
                !x.IsDeleted && x.Name.ToLower() == n);
        }

        public async Task<bool> ExistsPricelistNameAsync(string name, long typeId)
        {
            var n = (name ?? "").Trim().ToLower();
            return await _context.PriceLists.AnyAsync(p =>
                !p.IsDeleted && p.TypeId == typeId && p.Name.ToLower() == n);
        }

        public async Task<bool> HasOverlappingPeriodAsync(long typeId, DateTime from, DateTime to)
        {
            // Inclusive range overlap check: (aStart <= bEnd) && (bStart <= aEnd)
            return await _context.PriceLists.AnyAsync(p =>
                !p.IsDeleted &&
                p.TypeId == typeId &&
                p.EffectiveFrom <= to && from <= p.EffectiveTo);
        }

        private async Task EnsureTypeExists(long typeId)
        {
            var ok = await _context.PricelistTypes.AnyAsync(t => t.Id == typeId && !t.IsDeleted);
            if (!ok) throw new ApplicationException("Pricelist type not found.");
        }

        private async Task ValidatePricelistAsync(PriceList p, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(p.Name))
                throw new ApplicationException("Pricelist name cannot be empty.");

            if (p.EffectiveFrom == DateTime.MinValue || p.EffectiveTo == DateTime.MinValue)
                throw new ApplicationException("Effective dates must be set.");

            if (p.EffectiveFrom > p.EffectiveTo)
                throw new ApplicationException("EffectiveFrom cannot be after EffectiveTo.");

            await EnsureTypeExists(p.TypeId);

            // Unique name per type
            var nameExists = await ExistsPricelistNameAsync(p.Name, p.TypeId);
            if (nameExists)
                throw new ApplicationException($"A pricelist named '{p.Name}' already exists for this type.");
        }
    }
}
