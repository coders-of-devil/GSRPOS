using Applications.Interfaces.UserInterfaces;
using Domain.Entities.UserMod;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.UserImplementations
{
    public class RoleService(AppDbContext context, IDateService dateService,
        IIdGeneratorService idGeneratorService) : IRoleService
    {
        private readonly AppDbContext _context = context;
        private readonly IDateService _dateService = dateService;
        private readonly IIdGeneratorService _idGeneratorService = idGeneratorService;

        //list all the roles whether it's active or inactive
        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Where(r => !r.IsDeleted)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        //returns a role with the specified id
        public async Task<Role> GetByIdAsync(long id)
        {
            if(id == 0)
                throw new ApplicationException("role id is empty");

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                throw new ApplicationException($"role with id : {id} not found");

            return role;
        }
            

        //adding a new role 
        public async Task AddAsync(Role role)
        {
            //check whether the role name is null or empty
            if (string.IsNullOrWhiteSpace(role.Name))
                throw new ApplicationException("Role name is required.");

            //checks whether the role with the name exists or not
            var exists = await _context.Roles.AnyAsync(r => r.Name == role.Name && !r.IsDeleted);
            if (exists)
                throw new ApplicationException($"A role with the name '{role.Name}' already exists.");

            role.Id = await _idGeneratorService.GenerateNextId<Role>();
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            var dbRole = await _context.Roles
                .Where(r => r.Id == role.Id && !r.IsDeleted)
                .FirstOrDefaultAsync();

            //check whether the role exists or not
            if (dbRole == null)
                throw new ApplicationException("Role not found.");

            //check whether the role name is null or empty
            if (string.IsNullOrWhiteSpace(role.Name))
                throw new ApplicationException("Role name cannot be empty.");

            //checks whether the role with the name exists or not
            var nameExists = await _context.Roles
                .AnyAsync(r => r.Name == role.Name && r.Id != role.Id && !r.IsDeleted);
            if (nameExists)
                throw new ApplicationException($"Another role with the name '{role.Name}' already exists.");

            //updates the current data with the passed new data
            dbRole.Name = role.Name;
            dbRole.IsPermissionAssigned = role.IsPermissionAssigned;
            dbRole.IsSystemRole = role.IsSystemRole;
            dbRole.IsActive = role.IsActive;
            dbRole.ModifiedAt = _dateService.Now;

            _context.Roles.Entry(dbRole).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
        }


        public async Task DeleteAsync(long id)
        {
            var dbRole = await _context.Roles
                .Where(r => r.Id == id && !r.IsDeleted)
                .FirstOrDefaultAsync();

            //check whether the role exists or not
            if (dbRole == null)
                throw new ApplicationException("Role not found.");

            //check whether the role to delete is a system role or not
            if (dbRole.IsSystemRole)
                throw new ApplicationException("System roles cannot be deleted.");

            //soft deletion method
            dbRole.IsDeleted = true;
            dbRole.IsActive = false;
            dbRole.ModifiedAt = _dateService.Now;

            _context.Roles.Entry(dbRole).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
