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
    public class ModifierGroupService(AppDbContext ctx, IIdGeneratorService id,
        IDateService date) : IModifierGroupService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<List<ModifierGroup>> GetAllAsync() =>
            await _ctx.ModifierGroups
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name)
                .ToListAsync();

        public async Task<ModifierGroup> GetByIdAsync(long id) =>
            await _ctx.ModifierGroups.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Modifier group not found.");

        // -------- Existence --------
        public async Task<bool> ExistsByNameAsync(string name, long? excludeId = null)
        {
            var n = (name ?? "").Trim().ToLower();
            return await _ctx.ModifierGroups.AnyAsync(x =>
                !x.IsDeleted &&
                x.Name.ToLower() == n &&
                (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        // -------- Commands --------
        public async Task<long> AddAsync(ModifierGroup group)
        {
            await ValidateAsync(group, isUpdate: false);
            group.Id = await _id.GenerateNextId<ModifierGroup>();
            group.CreatedAt = _date.Now;
            _ctx.ModifierGroups.Add(group);
            await _ctx.SaveChangesAsync();
            return group.Id;
        }

        public async Task UpdateAsync(ModifierGroup group)
        {
            var existing = await GetByIdAsync(group.Id);
            await ValidateAsync(group, isUpdate: true);

            existing.Name = group.Name.Trim();
            existing.IsRequired = group.IsRequired;
            existing.MaxSelectable = group.MaxSelectable;
            existing.DisplayOrder = group.DisplayOrder;
            existing.Description = group.Description ?? string.Empty;
            existing.ModifiedAt = _date.Now;

            _ctx.ModifierGroups.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.ModifierGroups.Update(existing);

            // Soft-delete modifiers under this group as well (optional but practical)
            var modifiers = await _ctx.Modifiers
                .Where(m => !m.IsDeleted && m.ModifierGroupId == id)
                .ToListAsync();
            foreach (var m in modifiers) { m.IsDeleted = true; m.ModifiedAt = _date.Now; }
            if (modifiers.Count > 0) _ctx.Modifiers.UpdateRange(modifiers);

            await _ctx.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(ModifierGroup g, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(g.Name))
                throw new ApplicationException("Group name cannot be empty.");
            if (g.MaxSelectable < 0)
                throw new ApplicationException("Max selectable cannot be negative.");
            if (g.IsRequired && g.MaxSelectable < 1)
                throw new ApplicationException("If a group is required, MaxSelectable must be at least 1.");

            if (await ExistsByNameAsync(g.Name, isUpdate ? g.Id : null))
                throw new ApplicationException($"A modifier group named '{g.Name}' already exists.");
        }
    }
}
