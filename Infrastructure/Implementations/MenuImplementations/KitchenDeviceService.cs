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
    public class KitchenDeviceService(AppDbContext ctx, IIdGeneratorService id,
        IDateService date) : IKitchenDeviceService
    {
        private readonly AppDbContext _ctx = ctx;
        private readonly IIdGeneratorService _id = id;
        private readonly IDateService _date = date;

        // ---------- Queries ----------
        public async Task<List<KitchenDevice>> GetAllAsync() =>
            await _ctx.KitchenDevices
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.PreparationAreaId).ThenBy(x => x.Name)
                .ToListAsync();

        public async Task<KitchenDevice> GetByIdAsync(long id) =>
            await _ctx.KitchenDevices.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new ApplicationException("Kitchen device not found.");

        public async Task<List<KitchenDevice>> GetByPreparationAreaAsync(long preparationAreaId) =>
            await _ctx.KitchenDevices
                .Where(x => !x.IsDeleted && x.PreparationAreaId == preparationAreaId)
                .OrderBy(x => x.Name)
                .ToListAsync();

        public async Task<List<KitchenDevice>> GetActiveAsync() =>
            await _ctx.KitchenDevices
                .Where(x => !x.IsDeleted && x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();

        public async Task<List<KitchenDevice>> GetByTypeAsync(string type)
        {
            var t = (type ?? "").Trim().ToLower();
            return await _ctx.KitchenDevices
                .Where(x => !x.IsDeleted && x.Type.ToLower() == t)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        // ---------- Existence ----------
        public async Task<bool> ExistsByNameAsync(string name, long? preparationAreaId, long? excludeId = null)
        {
            var n = (name ?? "").Trim().ToLower();
                return await _ctx.KitchenDevices.AnyAsync(x =>
                    !x.IsDeleted &&
                    x.Name.ToLower() == n &&
                    x.PreparationAreaId == preparationAreaId &&
                    (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<bool> ExistsByNetworkAsync(string ipAddress, string port, long? excludeId = null)
        {
            var ip = (ipAddress ?? "").Trim().ToLower();
            var p = (port ?? "").Trim().ToLower();
            if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(p)) return false;

            return await _ctx.KitchenDevices.AnyAsync(x =>
                !x.IsDeleted &&
                x.IpAddress.ToLower() == ip &&
                x.Port.ToLower() == p &&
                (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        // ---------- Commands ----------
        public async Task<long> AddAsync(KitchenDevice device)
        {
            await ValidateAsync(device, isUpdate: false);
            device.Id = await _id.GenerateNextId<KitchenDevice>();
            device.CreatedAt = _date.Now;

            _ctx.KitchenDevices.Add(device);
            await _ctx.SaveChangesAsync();
            return device.Id;
        }

        public async Task UpdateAsync(KitchenDevice device)
        {
            var existing = await GetByIdAsync(device.Id);
            await ValidateAsync(device, isUpdate: true);

            existing.Name = device.Name.Trim();
            existing.Type = (device.Type ?? "").Trim();
            existing.ConnectionType = (device.ConnectionType ?? "").Trim();
            existing.IpAddress = device.IpAddress ?? "";
            existing.Port = device.Port ?? "";
            existing.Model = device.Model ?? "";
            existing.PreparationAreaId = device.PreparationAreaId; // may be null
            existing.IsActive = device.IsActive;
            existing.DefaultCopies = device.DefaultCopies;
            existing.ModifiedAt = _date.Now;

            _ctx.KitchenDevices.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await GetByIdAsync(id);
            existing.IsDeleted = true;
            existing.ModifiedAt = _date.Now;

            _ctx.KitchenDevices.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // ---------- Toggles / helpers ----------
        public async Task SetActiveAsync(long id, bool isActive)
        {
            var existing = await GetByIdAsync(id);
            existing.IsActive = isActive;
            existing.ModifiedAt = _date.Now;

            _ctx.KitchenDevices.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task SetDefaultCopiesAsync(long id, int copies)
        {
            var existing = await GetByIdAsync(id);
            if (copies < 0) throw new ApplicationException("Default copies cannot be negative.");
            existing.DefaultCopies = copies;
            existing.ModifiedAt = _date.Now;

            _ctx.KitchenDevices.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        public async Task AssignToAreaAsync(long id, long? preparationAreaId)
        {
            var existing = await GetByIdAsync(id);

            if (preparationAreaId.HasValue)
            {
                var areaOk = await _ctx.PreparationAreas.AnyAsync(a => a.Id == preparationAreaId.Value && !a.IsDeleted);
                if (!areaOk) throw new ApplicationException("Preparation area not found.");
            }

            existing.PreparationAreaId = preparationAreaId;
            existing.ModifiedAt = _date.Now;

            _ctx.KitchenDevices.Update(existing);
            await _ctx.SaveChangesAsync();
        }

        // ---------- Validation ----------
        private async Task ValidateAsync(KitchenDevice d, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(d.Name))
                throw new ApplicationException("Device name cannot be empty.");

            if (string.IsNullOrWhiteSpace(d.Type))
                throw new ApplicationException("Device type cannot be empty.");

            if (string.IsNullOrWhiteSpace(d.ConnectionType))
                throw new ApplicationException("Connection type cannot be empty.");

            if (d.DefaultCopies < 0)
                throw new ApplicationException("Default copies cannot be negative.");

            // If area is set, ensure it exists
            if (d.PreparationAreaId.HasValue)
            {
                var areaOk = await _ctx.PreparationAreas.AnyAsync(p => p.Id == d.PreparationAreaId && !p.IsDeleted);
                if (!areaOk) throw new ApplicationException("Preparation area not found.");
            }

            // Unique: Name within same area (including null area)
            if (await ExistsByNameAsync(d.Name, d.PreparationAreaId, isUpdate ? d.Id : null))
                throw new ApplicationException($"A device named '{d.Name}' already exists in this area.");

            // Network uniqueness if relevant
            var usesNetwork = (d.ConnectionType ?? "").Equals("Network", StringComparison.OrdinalIgnoreCase)
                              || (!string.IsNullOrWhiteSpace(d.IpAddress) || !string.IsNullOrWhiteSpace(d.Port));

            if (usesNetwork)
            {
                if (string.IsNullOrWhiteSpace(d.IpAddress) || string.IsNullOrWhiteSpace(d.Port))
                    throw new ApplicationException("IP address and port are required for network devices.");

                // basic sanity (you can replace with a stricter regex/IP parser)
                if (d.IpAddress.Length < 7) throw new ApplicationException("IP address looks invalid.");

                if (await ExistsByNetworkAsync(d.IpAddress, d.Port, isUpdate ? d.Id : null))
                    throw new ApplicationException($"Another device is already using {d.IpAddress}:{d.Port}.");
            }
        }
    }
}
