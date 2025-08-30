using Applications.Interfaces.UserInterfaces;
using Domain.DTOs;
using Domain.Entities.UserMod;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.UserImplementations
{
    public class RolePermissionService(AppDbContext context, IIdGeneratorService idGenerator,
        IDateService dateService) : IRolePermissionService
    {
        private readonly AppDbContext _context = context;
        private readonly IIdGeneratorService _idGenerator = idGenerator;
        private readonly IDateService _dateService = dateService;

        public async Task<List<RolePermission>> GetAllByRole(long r_Id)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Include(rp => rp.Role)
                .Where(rp => rp.RoleId == r_Id && rp.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<List<RolePermission>> GetAllByPermission(long p_Id)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .Where(rp => rp.PermissionId == p_Id && rp.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<RolePermission?> GetById(long id)
        {
            return await _context.RolePermissions.Where(rp => rp.IsDeleted == false && rp.Id == id)
                .Include(rp => rp.Role).Include(rp => rp.Permission)
                .FirstOrDefaultAsync();
        }

        public async Task CreateRolePermissionAsync(RolePermission rp)
        {
            if (rp.PermissionId == 0 || rp.RoleId == 0)
                throw new ApplicationException($"Either role or permission is missing.");

            var exist = await ExistsAsync(rp.RoleId, rp.PermissionId);
            if (exist)
                throw new ApplicationException($"Selected permission already exist for the role.");

            var dbRole = await _context.Roles.Where(e => e.IsDeleted == false && e.Id == rp.RoleId)
                .FirstOrDefaultAsync();
            if(dbRole != null && dbRole.IsPermissionAssigned == false)
            {
                dbRole.IsPermissionAssigned = true;
                dbRole.ModifiedAt = _dateService.Now;

                _context.Roles.Entry(dbRole).State = EntityState.Modified;
            }

            rp.Id = await _idGenerator.GenerateNextId<RolePermission>();
            _context.RolePermissions.Add(rp);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRolePermissionAsync(RolePermission rp)
        {
            var existing = await _context.RolePermissions.Where(e => e.IsDeleted == false && e.Id == rp.Id)
                .FirstOrDefaultAsync();
            if (existing == null)
                throw new ApplicationException($"RolePermission does not exist.");

            if (rp.PermissionId == 0 || rp.RoleId == 0)
                throw new ApplicationException($"Either role or permission is missing.");

            var exist = await ExistsAsync(rp.RoleId, rp.PermissionId);
            if (exist)
                throw new ApplicationException($"Selected permission already exist for the role.");

            existing.PermissionId = rp.PermissionId;
            existing.RoleId = rp.RoleId;
            existing.ModifiedAt = _dateService.Now;

            _context.RolePermissions.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRolePermissionAsync(long id)
        {
            if (id == 0)
                throw new ApplicationException($"Permission missing for the role");

            var existing = await _context.RolePermissions.Where(e => e.IsDeleted == false && e.Id == id)
                .FirstOrDefaultAsync();
            if (existing == null)
                throw new ApplicationException($"RolePermission does not exist.");

            existing.IsDeleted = true;
            existing.ModifiedAt = _dateService.Now;

            _context.RolePermissions.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(long r_Id, long p_Id)
        {
            return await _context.RolePermissions
            .AnyAsync(rp => rp.RoleId == r_Id && rp.PermissionId == p_Id && rp.IsDeleted == false);
        }

        public async Task AssignPermissionsToRoleAsync(long roleId, List<long> permissionIds)
        {
            if (roleId == 0 || permissionIds == null || !permissionIds.Any())
                throw new ApplicationException("Role or permission list is missing.");

            foreach (var pid in permissionIds)
            {
                try
                {
                    await CreateRolePermissionAsync(new RolePermission()
                    {
                        RoleId = roleId,
                        PermissionId = pid
                    });
                }
                catch (ApplicationException ex)
                {
                    throw new ApplicationException(ex.Message);
                }
            }
        }

        public async Task RemoveAllPermissionsFromRoleAsync(long id)
        {
            if (id == 0)
                throw new ApplicationException($"role is missing");

            var existing = await _context.RolePermissions.Where(rp => rp.RoleId == id && rp.IsDeleted == false)
                .ToListAsync();

            if (existing.Any())
            {
                foreach (var rp in existing)
                {
                    rp.IsDeleted = true;
                    rp.ModifiedAt = _dateService.Now;

                    _context.RolePermissions.Entry(rp).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }  
        }

        public async Task<List<RolePermissionAssignmentDTO>> GetAssignablePermissionsAsync(long roleId)
        {
            var allPermissions = await _context.Permissions
                .Include(p => p.PermissionGroup)
                .ToListAsync();

            var assignedPermissionIds = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            return allPermissions.Select(p => new RolePermissionAssignmentDTO
            {
                PermissionId = p.Id,
                Action = p.Action,
                GroupName = p.PermissionGroup?.Name ?? "Uncategorized",
                IsSelected = assignedPermissionIds.Contains(p.Id)
            }).ToList();
        }
    }
}
