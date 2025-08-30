using Applications.Interfaces.MenuInterfaces;
using Domain.Entities.MenuMod;
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

namespace Infrastructure.Implementations.MenuImplementations
{
    public class PreparationAreaService(AppDbContext context, IIdGeneratorService idGeneratorService, 
        IDateService dateService) : IPreparationAreaService
    {
        private readonly IDateService _dateService = dateService;
        private readonly AppDbContext _context = context;
        private readonly IIdGeneratorService _idGeneratorService = idGeneratorService;

        public async Task<List<PreparationArea>> GetAllPreparationAreaAsync()
        {
            return await _context.PreparationAreas
                .Where(r => !r.IsDeleted)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<PreparationArea> GetPreparationAreaByIdAsync(long id)
        {
            if (id == 0)
                throw new ApplicationException("preparation area id is empty");

            var parea = await _context.PreparationAreas.Where(p => p.Id == id && p.IsDeleted == false)
                .FirstOrDefaultAsync();
            if (parea == null)
                throw new ApplicationException($"preparation area with id : {id} not found");

            return parea;
        }

        public async Task CreatePreparationAreaAsync(PreparationArea preparationArea)
        {
            //check whether the name is null or empty
            if (string.IsNullOrWhiteSpace(preparationArea.Name))
                throw new ApplicationException("Role name is required.");

            //checks whether any record with the name exists or not
            var exists = await _context.Roles.AnyAsync(r => r.Name == preparationArea.Name && !r.IsDeleted);
            if (exists)
                throw new ApplicationException($"A role with the name '{preparationArea.Name}' already exists.");

            preparationArea.Id = await _idGeneratorService.GenerateNextId<PreparationArea>();
            _context.PreparationAreas.Add(preparationArea);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePreparationAreaAsync(PreparationArea preparationArea)
        {
            var dbPArea = await _context.PreparationAreas
                .Where(r => r.Id == preparationArea.Id && !r.IsDeleted)
                .FirstOrDefaultAsync();

            //check whether the record exists or not
            if (dbPArea == null)
                throw new ApplicationException("Permission Area not found.");

            //check whether the role name is null or empty
            if (string.IsNullOrWhiteSpace(preparationArea.Name))
                throw new ApplicationException("Preparation Area name cannot be empty.");

            //checks whether the role with the name exists or not
            var nameExists = await _context.PreparationAreas
                .AnyAsync(r => r.Name == preparationArea.Name && r.Id != preparationArea.Id && !r.IsDeleted);
            if (nameExists)
                throw new ApplicationException($"Another role with the name '{preparationArea.Name}' already exists.");

            //updates the current data with the passed new data
            dbPArea.Name = preparationArea.Name;
            dbPArea.Remarks = preparationArea.Remarks;
            dbPArea.ModifiedAt = _dateService.Now;

            _context.PreparationAreas.Entry(dbPArea).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePreparationAreaAsync(long id)
        {
            if (id == 0)
                throw new ApplicationException($"Preparation Area is missing.");

            var dbPArea = await _context.PreparationAreas.FirstOrDefaultAsync(p => p.Id == id);

            if (dbPArea == null)
                throw new ApplicationException("Preparation Area not found.");

            dbPArea.IsDeleted = true;
            dbPArea.ModifiedAt = _dateService.Now;

            _context.PreparationAreas.Entry(dbPArea).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
