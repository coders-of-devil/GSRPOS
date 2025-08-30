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
    public class MenuItemTypeService : IMenuItemTypeService
    {
        private readonly AppDbContext _ctx;
        private readonly IIdGeneratorService _id;
        private readonly IDateService _date;

        public MenuItemTypeService(AppDbContext ctx, IIdGeneratorService id, IDateService date)
        { _ctx = ctx; _id = id; _date = date; }

        public async Task<List<MenuItemType>> GetByItemAsync(long itemId) =>
            await _ctx.MenuItemTypes.Where(x => !x.IsDeleted && x.ItemId == itemId)
                .OrderBy(x => x.PricelistId).ThenBy(x => x.TypeName)
                .ToListAsync();

        public async Task<List<MenuItemType>> GetByItemAndPricelistAsync(long itemId, long pricelistId) =>
            await _ctx.MenuItemTypes.Where(x => !x.IsDeleted && x.ItemId == itemId && x.PricelistId == pricelistId)
                .OrderBy(x => x.TypeName).ToListAsync();

        public async Task<MenuItemType> GetByIdAsync(long id) =>
            await _ctx.MenuItemTypes.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Menu item type not found.");

        public async Task<bool> ExistsAsync(long itemId, long pricelistId, string typeName, long? excludeId = null)
        {
            var n = (typeName ?? "").Trim().ToLower();
            return await _ctx.MenuItemTypes.AnyAsync(x =>
                !x.IsDeleted && x.ItemId == itemId && x.PricelistId == pricelistId &&
                x.TypeName.ToLower() == n &&
                (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<long> AddAsync(MenuItemType type)
        {
            await ValidateAsync(type, false);
            type.Id = await _id.GenerateNextId<MenuItemType>();
            type.CreatedAt = _date.Now;
            _ctx.MenuItemTypes.Add(type);
            await _ctx.SaveChangesAsync();
            return type.Id;
        }

        public async Task UpdateAsync(MenuItemType type)
        {
            var existing = await GetByIdAsync(type.Id);
            await ValidateAsync(type, true);

            existing.ItemId = type.ItemId;
            existing.PricelistId = type.PricelistId;
            existing.TypeName = type.TypeName.Trim();
            existing.Price = type.Price;
            existing.Description = type.Description ?? "";
            existing.ModifiedAt = _date.Now;

            _ctx.MenuItemTypes.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.MenuItemTypes.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        private async Task ValidateAsync(MenuItemType t, bool isUpdate)
        {
            if (t.Price < 0) throw new ApplicationException("Type price cannot be negative.");
            if (string.IsNullOrWhiteSpace(t.TypeName))
                throw new ApplicationException("Type name cannot be empty.");

            var itemOk = await _ctx.MenuItems.AnyAsync(i => i.Id == t.ItemId && !i.IsDeleted);
            if (!itemOk) throw new ApplicationException("Menu item not found.");

            var plOk = await _ctx.PriceLists.AnyAsync(p => p.Id == t.PricelistId && !p.IsDeleted);
            if (!plOk) throw new ApplicationException("Pricelist not found.");

            if (await ExistsAsync(t.ItemId, t.PricelistId, t.TypeName, isUpdate ? t.Id : null))
                throw new ApplicationException("A type with this name already exists on this pricelist for the item.");
        }
    }

}
