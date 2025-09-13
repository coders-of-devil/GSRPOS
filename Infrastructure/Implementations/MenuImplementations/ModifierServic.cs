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
    public class ModifierService(AppDbContext ctx, IIdGeneratorService id,
        IDateService date) : IModifierService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<List<Modifier>> GetByGroupAsync(long modifierGroupId) =>
            await _ctx.Modifiers
                .Where(x => !x.IsDeleted && x.ModifierGroupId == modifierGroupId)
                .OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name)
                .ToListAsync();

        public async Task<Modifier> GetByIdAsync(long id) =>
            await _ctx.Modifiers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Modifier not found.");

        public async Task<List<Modifier>> GetAllAsync() =>
            await _ctx.Modifiers
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name)
                .ToListAsync();

        // -------- Existence --------
        public async Task<bool> ExistsByNameAsync(string name, long modifierGroupId, long? excludeId = null)
        {
            var n = (name ?? "").Trim().ToLower();
            return await _ctx.Modifiers.AnyAsync(x =>
                !x.IsDeleted &&
                x.ModifierGroupId == modifierGroupId &&
                x.Name.ToLower() == n &&
                (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        // -------- Commands --------
        public async Task<long> AddAsync(Modifier modifier)
        {
            await ValidateAsync(modifier, isUpdate: false);
            modifier.Id = await _id.GenerateNextId<Modifier>();
            modifier.CreatedAt = _date.Now;
            _ctx.Modifiers.Add(modifier);
            await _ctx.SaveChangesAsync();
            return modifier.Id;
        }

        public async Task UpdateAsync(Modifier modifier)
        {
            var existing = await GetByIdAsync(modifier.Id);
            await ValidateAsync(modifier, isUpdate: true);

            existing.ModifierGroupId = modifier.ModifierGroupId;
            existing.Name = modifier.Name.Trim();
            existing.Price = modifier.Price;
            existing.DisplayOrder = modifier.DisplayOrder;
            existing.IsDefault = modifier.IsDefault;
            existing.ModifiedAt = _date.Now;

            _ctx.Modifiers.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.Modifiers.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(Modifier m, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(m.Name))
                throw new ApplicationException("Modifier name cannot be empty.");
            if (m.Price < 0)
                throw new ApplicationException("Modifier price cannot be negative.");
            if (m.DisplayOrder < 0)
                throw new ApplicationException("Display order cannot be negative.");

            var groupOk = await _ctx.ModifierGroups.AnyAsync(g => g.Id == m.ModifierGroupId && !g.IsDeleted);
            if (!groupOk) throw new ApplicationException("Modifier group not found.");

            if (await ExistsByNameAsync(m.Name, m.ModifierGroupId, isUpdate ? m.Id : null))
                throw new ApplicationException($"A modifier named '{m.Name}' already exists in this group.");
        }
    }
}
