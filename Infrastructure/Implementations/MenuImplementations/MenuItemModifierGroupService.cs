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
    public class MenuItemModifierGroupService(AppDbContext ctx, IIdGeneratorService id,
        IDateService date) : IMenuItemModifierGroupService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // -------- Queries --------
        public async Task<List<MenuItemModifierGroup>> GetByMenuItemAsync(long menuItemId) =>
            await _ctx.MenuItemModifierGroups
                .Where(x => !x.IsDeleted && x.MenuItemId == menuItemId)
                .OrderBy(x => x.Id)
                .ToListAsync();

        public async Task<MenuItemModifierGroup> GetByIdAsync(long id) =>
            await _ctx.MenuItemModifierGroups.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Menu item ↔ modifier group link not found.");

        // -------- Existence --------
        public async Task<bool> ExistsAsync(long menuItemId, long modifierGroupId, long? excludeId = null) =>
            await _ctx.MenuItemModifierGroups.AnyAsync(x =>
                !x.IsDeleted &&
                x.MenuItemId == menuItemId &&
                x.ModifierGroupId == modifierGroupId &&
                (!excludeId.HasValue || x.Id != excludeId.Value));

        // -------- Commands --------
        public async Task<long> AddAsync(MenuItemModifierGroup link)
        {
            await ValidateAsync(link, isUpdate: false);
            link.Id = await _id.GenerateNextId<MenuItemModifierGroup>();
            link.CreatedAt = _date.Now;
            _ctx.MenuItemModifierGroups.Add(link);
            await _ctx.SaveChangesAsync();
            return link.Id;
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.MenuItemModifierGroups.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // -------- Validation --------
        private async Task ValidateAsync(MenuItemModifierGroup link, bool isUpdate)
        {
            var itemOk = await _ctx.MenuItems.AnyAsync(m => m.Id == link.MenuItemId && !m.IsDeleted);
            if (!itemOk) throw new ApplicationException("Menu item not found.");

            var groupOk = await _ctx.ModifierGroups.AnyAsync(g => g.Id == link.ModifierGroupId && !g.IsDeleted);
            if (!groupOk) throw new ApplicationException("Modifier group not found.");

            if (await ExistsAsync(link.MenuItemId, link.ModifierGroupId, isUpdate ? link.Id : null))
                throw new ApplicationException("This MenuItem already uses that ModifierGroup.");
        }
    }
}
