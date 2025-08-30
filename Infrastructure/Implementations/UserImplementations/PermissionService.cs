using Applications.Interfaces.UserInterfaces;
using Domain.Entities.UserMod;
using Domain.Enums;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.UserImplementations
{
    public class PermissionService(AppDbContext context, IIdGeneratorService idGenerator,
        IDateService dateService) : IPermissionService
    {
        private readonly AppDbContext _context = context;
        private readonly IDateService _dateService = dateService;
        private readonly IIdGeneratorService _idGenerator = idGenerator;

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _context.Permissions
                .Include(p => p.PermissionGroup)
                .OrderBy(p => p.PermissionGroup!.Name)
                .ThenBy(p => p.Action)
                .ToListAsync();
        }

        public async Task<Permission> GetByIdAsync(long id)
        {
            return await _context.Permissions
                .Include(p => p.PermissionGroup)
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new ApplicationException("Permission not found.");
        }

        public async Task<List<Permission>> GetByPermissionGroupAsync(long permissionGroupId)
        {
            return await _context.Permissions
                .Where(p => p.PermissionGroupId == permissionGroupId)
                .OrderBy(p => p.Action)
                .ToListAsync();
        }

        public Task<List<PermissionActionEnum>> GetAvailableActionsAsync()
        {
            var actions = Enum.GetValues<PermissionActionEnum>().ToList();
            return Task.FromResult(actions);
        }

        public async Task<bool> ExistsAsync(long permissionGroupId, string action)
        {
            return await _context.Permissions
                .AnyAsync(p => p.PermissionGroupId == permissionGroupId && p.Action == action);
        }

        public async Task AddAsync(Permission permission)
        {
            if (permission.PermissionGroupId == 0 || permission.Action == null)
                throw new ApplicationException($"Permission or Action is missing.");

            var exists = await ExistsAsync(permission.PermissionGroupId, permission.Action);
            if (exists)
                throw new ApplicationException($"Permission '{permission.Action}' already exists for " +
                    $"the selected group.");

            permission.Id = await _idGenerator.GenerateNextId<Permission>();
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            var dbPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Id == permission.Id);

            if (dbPermission == null)
                throw new ApplicationException("Permission not found.");

            if (permission.PermissionGroupId == 0 || permission.Action == null)
                throw new ApplicationException($"Permission or Action is missing.");

            var exists = await ExistsAsync(permission.PermissionGroupId, permission.Action);
            if (exists)
                throw new ApplicationException($"Permission '{permission.Action}' already exists for " +
                    $"the selected group.");

            dbPermission.PermissionGroupId = permission.PermissionGroupId;
            dbPermission.Action = permission.Action;
            dbPermission.Description = permission.Description;
            dbPermission.ModifiedAt = _dateService.Now;

            _context.Permissions.Entry(dbPermission).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            if (id == 0)
                throw new ApplicationException($"Permission is missing.");

            var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.Id == id) 
                ?? throw new ApplicationException("Permission not found.");
            permission.IsDeleted = true;
            permission.ModifiedAt = _dateService.Now;

            _context.Permissions.Entry(permission).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
