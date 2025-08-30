using Applications.Interfaces.UserInterfaces;
using Domain.Entities.UserMod;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.ServiceClass;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.UserImplementations
{
    public class PermissionGroupService(AppDbContext context, IDateService dateService,
        IIdGeneratorService idGenerator) : IPermissionGroupService
    {
        private readonly AppDbContext _context = context;
        private readonly IDateService _dateService = dateService;
        private readonly IIdGeneratorService _idGenerator = idGenerator;

        public async Task<List<PermissionGroup>> GetAllAsync()
        {
            return await _context.PermissionGroups
                .Where(r => !r.IsDeleted)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<PermissionGroup> GetByIdAsync(long id) =>
            await _context.PermissionGroups.FindAsync(id);

        public async Task AddAsync(PermissionGroup permissionGroup)
        {
            //check code exist or not
            var codeExists = await _context.PermissionGroups.AnyAsync(e => e.Code == permissionGroup.Code
                && !e.IsDeleted);
            if (codeExists)
                throw new ApplicationException("permission group code already exist");
            //check name exist or not
            var nameExists =  await _context.PermissionGroups.AnyAsync(e => e.Name == permissionGroup.Name
                && !e.IsDeleted);
            if (nameExists)
                throw new ApplicationException("permission group name already exist");

            //check whether name is empty or not
            if (string.IsNullOrWhiteSpace(permissionGroup.Name))
                throw new ApplicationException("permission group name cannot be empty.");

            permissionGroup.Id = await _idGenerator.GenerateNextId<PermissionGroup>();
            _context.PermissionGroups.Add(permissionGroup);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PermissionGroup permissionGroup)
        {
            var dbPermissionGroup = await _context.PermissionGroups
                .Where(r => r.Id == permissionGroup.Id && !r.IsDeleted)
                .FirstOrDefaultAsync();

            if (dbPermissionGroup == null)
                throw new ApplicationException("permission group not found");

            //check whether name is empty or not
            if (string.IsNullOrWhiteSpace(permissionGroup.Name))
                throw new ApplicationException("permission group name cannot be empty.");

            var nameExists = await _context.PermissionGroups
                .AnyAsync(r => r.Name == permissionGroup.Name && r.Id != permissionGroup.Id && !r.IsDeleted);
            if (nameExists)
                throw new ApplicationException($"Another role with the name '{permissionGroup.Name}' " +
                    $"already exists.");

            dbPermissionGroup.Name = permissionGroup.Name;
            dbPermissionGroup.IsModelAssociated = permissionGroup.IsModelAssociated;
            dbPermissionGroup.ModifiedAt = _dateService.Now;

            _context.PermissionGroups.Entry(dbPermissionGroup).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var dbPermissionGroup = await _context.PermissionGroups
                .Where(r => r.Id == id && !r.IsDeleted)
                .FirstOrDefaultAsync();

            if (dbPermissionGroup == null)
                throw new ApplicationException("permission group not found");

            dbPermissionGroup.IsDeleted = true;
            dbPermissionGroup.ModifiedAt = _dateService.Now;

            _context.PermissionGroups.Entry(dbPermissionGroup).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
