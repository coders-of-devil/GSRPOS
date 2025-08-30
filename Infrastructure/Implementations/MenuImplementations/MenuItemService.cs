using Applications.Interfaces.MenuInterfaces;
using Domain.Entities.MenuMod;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.MenuImplementations
{
    public class MenuItemService : IMenuItemService
    {
        private readonly AppDbContext _ctx;
        private readonly IIdGeneratorService _id;
        private readonly IDateService _date;

        public MenuItemService(AppDbContext ctx, IIdGeneratorService id, IDateService date)
        { _ctx = ctx; _id = id; _date = date; }

        public async Task<List<MenuItem>> GetAllAsync() =>
            await _ctx.MenuItems.Where(x => !x.IsDeleted)
                .OrderBy(x => x.CategoryId).ThenBy(x => x.DisplayOrder)
                .ToListAsync();

        public async Task<MenuItem> GetByIdAsync(long id) =>
            await _ctx.MenuItems.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Menu item not found.");

        public async Task<List<MenuItem>> GetByCategoryAsync(long categoryId) =>
            await _ctx.MenuItems.Where(x => !x.IsDeleted && x.CategoryId == categoryId)
                .OrderBy(x => x.DisplayOrder).ToListAsync();

        public async Task<List<MenuItem>> GetByPreparationAreaAsync(long preparationAreaId) =>
            await _ctx.MenuItems.Where(x => !x.IsDeleted && x.PreparationAreaId == preparationAreaId)
                .OrderBy(x => x.DisplayOrder).ToListAsync();

        public async Task<bool> ExistByNameAsync(string name, long categoryId, long? excludeId = null)
        {
            var n = (name ?? "").Trim().ToLower();
            return await _ctx.MenuItems.AnyAsync(x =>
                !x.IsDeleted && x.CategoryId == categoryId &&
                x.Name.ToLower() == n &&
                (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<bool> ExistByOrderNoAsync(int orderNo, long categoryId, long? excludeId = null) =>
            await _ctx.MenuItems.AnyAsync(x =>
                !x.IsDeleted && x.CategoryId == categoryId &&
                x.DisplayOrder == orderNo &&
                (!excludeId.HasValue || x.Id != excludeId.Value));

        public async Task<long> AddAsync(MenuItem item)
        {
            await ValidateAsync(item, false);
            item.Id = await _id.GenerateNextId<MenuItem>();
            item.CreatedAt = _date.Now;
            item.StockKeepingUnit = "Pcs";
            _ctx.MenuItems.Add(item);
            await _ctx.SaveChangesAsync();
            return item.Id;
        }

        public async Task UpdateAsync(MenuItem item)
        {
            var existing = await GetByIdAsync(item.Id);
            await ValidateAsync(item, true);

            existing.Name = item.Name.Trim();
            existing.Description = item.Description ?? "";
            existing.CategoryId = item.CategoryId;
            existing.TaxRate = item.TaxRate;
            existing.Barcode = item.Barcode ?? "";
            existing.ImagePath = item.ImagePath ?? "";
            existing.DisplayOrder = item.DisplayOrder;
            existing.ExpectedPreparationTime = item.ExpectedPreparationTime;
            existing.PreparationAreaId = item.PreparationAreaId;
            existing.IsActive = item.IsActive;
            existing.IsAvailableToday = item.IsAvailableToday;
            existing.ModifiedAt = _date.Now;

            _ctx.MenuItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.MenuItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task MarkNotAvailableTodayAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsAvailableToday = false;
            existing.ModifiedAt = _date.Now;
            _ctx.MenuItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task SetActiveAsync(long id, bool isActive)
        {
            var existing = await GetByIdAsync(id);
            existing.IsActive = isActive;
            existing.ModifiedAt = _date.Now;
            _ctx.MenuItems.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        private async Task ValidateAsync(MenuItem i, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(i.Name))
                throw new ApplicationException("Item name cannot be empty.");
            if (i.DisplayOrder < 0 || i.ExpectedPreparationTime < 0)
                throw new ApplicationException("Display order or preparation time cannot be negative.");
            if (i.TaxRate < 0 || i.TaxRate > 100)
                throw new ApplicationException("Tax rate must be between 0 and 100.");

            var catOk = await _ctx.MenuCategories.AnyAsync(c => c.Id == i.CategoryId && !c.IsDeleted);
            if (!catOk) throw new ApplicationException("Category not found.");

            var prepOk = await _ctx.PreparationAreas.AnyAsync(p => p.Id == i.PreparationAreaId && !p.IsDeleted);
            if (!prepOk) throw new ApplicationException("Preparation area not found.");

            if (await ExistByNameAsync(i.Name, i.CategoryId, isUpdate ? i.Id : null))
                throw new ApplicationException($"An item named '{i.Name}' already exists in this category.");

            if (await ExistByOrderNoAsync(i.DisplayOrder, i.CategoryId, isUpdate ? i.Id : null))
                throw new ApplicationException($"Display order {i.DisplayOrder} is already used in this category.");

            if (!string.IsNullOrWhiteSpace(i.Barcode))
            {
                var bc = i.Barcode.Trim().ToLower();
                var bcDup = await _ctx.MenuItems.AnyAsync(x =>
                    !x.IsDeleted && x.Barcode.ToLower() == bc &&
                    (!isUpdate || x.Id != i.Id));
                if (bcDup) throw new ApplicationException($"Barcode '{i.Barcode}' is already in use.");
            }
        }
    }
}

