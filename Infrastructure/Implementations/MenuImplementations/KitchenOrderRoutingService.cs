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
    public class KitchenOrderRoutingService(AppDbContext ctx, IIdGeneratorService id,
        IDateService date) : IKitchenOrderRoutingService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // --------- Queries ---------
        public async Task<List<KitchenOrderRouting>> GetAllAsync() =>
            await _ctx.KitchenOrderRoutings
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.MenuItemId).ThenBy(x => x.KitchenDeviceId)
                .ToListAsync();

        public async Task<KitchenOrderRouting> GetByIdAsync(long id) =>
            await _ctx.KitchenOrderRoutings.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Routing not found.");

        public async Task<List<KitchenOrderRouting>> GetByMenuItemAsync(long menuItemId) =>
        await _ctx.KitchenOrderRoutings
            .Where(x => !x.IsDeleted && x.MenuItemId == menuItemId)
            .OrderBy(x => x.KitchenDeviceId)
            .ToListAsync();

        public async Task<List<KitchenOrderRouting>> GetByKitchenDeviceAsync(long kitchenDeviceId) =>
            await _ctx.KitchenOrderRoutings
                .Where(x => !x.IsDeleted && x.KitchenDeviceId == kitchenDeviceId)
                .OrderBy(x => x.MenuItemId)
                .ToListAsync();

        public async Task<KitchenOrderRouting?> GetRouteAsync(long menuItemId, long kitchenDeviceId) =>
            await _ctx.KitchenOrderRoutings
                .FirstOrDefaultAsync(x => !x.IsDeleted &&
                                          x.MenuItemId == menuItemId &&
                                          x.KitchenDeviceId == kitchenDeviceId);

        // --------- Existence ---------
        public async Task<bool> ExistsAsync(long menuItemId, long kitchenDeviceId, long? excludeId = null) =>
            await _ctx.KitchenOrderRoutings.AnyAsync(x =>
                !x.IsDeleted &&
                x.MenuItemId == menuItemId &&
                x.KitchenDeviceId == kitchenDeviceId &&
                (!excludeId.HasValue || x.Id != excludeId.Value));

        // --------- Commands ---------
        public async Task<long> AddAsync(KitchenOrderRouting route)
        {
            await ValidateAsync(route, isUpdate: false);
            route.Id = await _id.GenerateNextId<KitchenOrderRouting>();
            route.CreatedAt = _date.Now;
            _ctx.KitchenOrderRoutings.Add(route);
            await _ctx.SaveChangesAsync();
            return route.Id;
        }

        public async Task UpdateAsync(KitchenOrderRouting route)
        {
            var existing = await GetByIdAsync(route.Id);
            await ValidateAsync(route, isUpdate: true);

            existing.MenuItemId = route.MenuItemId;
            existing.KitchenDeviceId = route.KitchenDeviceId;
            existing.Copies = route.Copies;
            existing.ModifiedAt = _date.Now;

            _ctx.KitchenOrderRoutings.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;
            _ctx.KitchenOrderRoutings.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // --------- Validation ---------
        private async Task ValidateAsync(KitchenOrderRouting r, bool isUpdate)
        {
            if (r.Copies < 1)
                throw new ApplicationException("Copies must be at least 1.");

            var itemOk = await _ctx.MenuItems.AnyAsync(m => m.Id == r.MenuItemId && !m.IsDeleted);
            if (!itemOk) throw new ApplicationException("Menu item not found.");

            var devOk = await _ctx.KitchenDevices.AnyAsync(d => d.Id == r.KitchenDeviceId && !d.IsDeleted);
            if (!devOk) throw new ApplicationException("Kitchen device not found.");

            // uniqueness on (MenuItemId, KitchenDeviceId)
            if (await ExistsAsync(r.MenuItemId, r.KitchenDeviceId, isUpdate ? r.Id : null))
                throw new ApplicationException("A routing for this MenuItem and KitchenDevice already exists.");
        }
    }
}
