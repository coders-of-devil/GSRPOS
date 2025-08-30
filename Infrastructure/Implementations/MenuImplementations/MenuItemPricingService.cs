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
    public class MenuItemPricingService : IMenuItemPricingService
    {
        private readonly AppDbContext _ctx;
        private readonly IIdGeneratorService _id;
        private readonly IDateService _date;

        public MenuItemPricingService(AppDbContext ctx, IIdGeneratorService id, IDateService date)
        { _ctx = ctx; _id = id; _date = date; }

        public async Task<MenuItemPriceListEntry> GetByIdAsync(long id) =>
            await _ctx.MenuItemPriceListEntries.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Pricelist entry not found.");

        public async Task<List<MenuItemPriceListEntry>> GetByPriceListAsync(long priceListId) =>
            await _ctx.MenuItemPriceListEntries.Where(x => !x.IsDeleted && x.PriceListId == priceListId)
                .OrderBy(x => x.MenuItemId).ToListAsync();

        public async Task<List<MenuItemPriceListEntry>> GetByItemAsync(long itemId) =>
            await _ctx.MenuItemPriceListEntries.Where(x => !x.IsDeleted && x.MenuItemId == itemId)
                .OrderBy(x => x.PriceListId).ToListAsync();

        public async Task<bool> ExistsAsync(long priceListId, long itemId, long? excludeId = null) =>
            await _ctx.MenuItemPriceListEntries.AnyAsync(x =>
                !x.IsDeleted && x.PriceListId == priceListId && x.MenuItemId == itemId &&
                (!excludeId.HasValue || x.Id != excludeId.Value));

        public async Task<long> AddAsync(MenuItemPriceListEntry entry)
        {
            await ValidateAsync(entry, false);
            entry.Id = await _id.GenerateNextId<MenuItemPriceListEntry>();
            entry.CreatedAt = _date.Now;
            entry.Notes ??= string.Empty;

            _ctx.MenuItemPriceListEntries.Add(entry);
            await _ctx.SaveChangesAsync();
            return entry.Id;
        }

        public async Task UpdateAsync(MenuItemPriceListEntry entry)
        {
            var existing = await GetByIdAsync(entry.Id);
            await ValidateAsync(entry, true);

            existing.PriceListId = entry.PriceListId;
            existing.MenuItemId = entry.MenuItemId;
            existing.Price = entry.Price;
            existing.Notes = entry.Notes ?? string.Empty;
            existing.ModifiedAt = _date.Now;

            _ctx.MenuItemPriceListEntries.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.MenuItemPriceListEntries.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task<decimal> GetEffectiveBasePriceAsync(long itemId, DateTime atUtc)
        {
            // If you consider PriceList.IsActive + date windows:
            var p = await (from e in _ctx.MenuItemPriceListEntries
                           join pl in _ctx.PriceLists on e.PriceListId equals pl.Id
                           where !e.IsDeleted && !pl.IsDeleted &&
                                 pl.IsActive &&
                                 pl.EffectiveFrom <= atUtc && atUtc <= pl.EffectiveTo &&
                                 e.MenuItemId == itemId
                           orderby pl.EffectiveFrom descending, e.ModifiedAt descending
                           select e.Price).FirstOrDefaultAsync();

            if (p > 0) return p;

            // no effective entry → fallback: require default list (Id=1) if present
            var fallback = await _ctx.MenuItemPriceListEntries
                .Where(e => !e.IsDeleted && e.MenuItemId == itemId && e.PriceListId == 1)
                .Select(e => e.Price).FirstOrDefaultAsync();

            if (fallback <= 0) throw new ApplicationException("No price found for the item.");
            return fallback;
        }

        private async Task ValidateAsync(MenuItemPriceListEntry e, bool isUpdate)
        {
            if (e.Price < 0) throw new ApplicationException("Price cannot be negative.");

            var itemOk = await _ctx.MenuItems.AnyAsync(i => i.Id == e.MenuItemId && !i.IsDeleted);
            if (!itemOk) throw new ApplicationException("Menu item not found.");

            var plOk = await _ctx.PriceLists.AnyAsync(p => p.Id == e.PriceListId && !p.IsDeleted);
            if (!plOk) throw new ApplicationException("Pricelist not found.");

            if (await ExistsAsync(e.PriceListId, e.MenuItemId, isUpdate ? e.Id : null))
                throw new ApplicationException("This item already has a base price on the selected pricelist.");
        }
    }

}
