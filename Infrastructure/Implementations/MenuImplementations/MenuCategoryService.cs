using Applications.Interfaces.MenuInterfaces;
using Domain.Entities.MenuMod;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.MenuImplementations
{
    public class MenuCategoryService(AppDbContext context, IIdGeneratorService idGenerator, 
        IDateService dateService) : IMenuCategoryService
    {
        private readonly AppDbContext _context = context;
        private readonly IIdGeneratorService _idGenerator = idGenerator;
        private readonly IDateService _dateService = dateService;

        public async Task<List<MenuCategory>> GetAllAsync()
        {
            return await _context.MenuCategories
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<MenuCategory?> GetByIdAsync(long id)
        {
            return await _context.MenuCategories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<List<MenuCategory>> GetParentCategoriesAsync()
        {
            return await _context.MenuCategories
                .Where(c => c.IsParentCategory && !c.IsDeleted)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<MenuCategory>> GetChildCategoriesAsync(long parentId)
        {
            return await _context.MenuCategories
                .Where(c => c.ParentCategoryId == parentId && !c.IsDeleted)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.MenuCategories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower() && !c.IsDeleted);
        }

        public async Task<bool> ExistsByOrderNumberAsync(int orderNo, bool isParentCategory)
        {
            if(isParentCategory)
                return await _context.MenuCategories
                    .AnyAsync(c => c.DisplayOrder == orderNo && !c.IsDeleted && c.IsParentCategory);
            else
                return await _context.MenuCategories
                    .AnyAsync(c => c.DisplayOrder == orderNo && !c.IsDeleted && !c.IsParentCategory);
        }

        public async Task<bool> HasChildCategoriesAsync(long parentId)
        {
            return await _context.MenuCategories
                .AnyAsync(c => c.ParentCategoryId == parentId && !c.IsDeleted);
        }

        public async Task AddAsync(MenuCategory category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ApplicationException("Category name cannot be empty");

            if (await ExistsByNameAsync(category.Name))
                throw new ApplicationException($"Category with name '{category.Name}' already exists");

            if (await ExistsByOrderNumberAsync(category.DisplayOrder, category.IsParentCategory))
                throw new ApplicationException($"Category with display order '{category.DisplayOrder}' " +
                    $"already exists");

            if (!category.IsParentCategory && category.ParentCategoryId == null)
                throw new ApplicationException("Child category must have a parent");

            category.Id = await _idGenerator.GenerateNextId<MenuCategory>();
            _context.MenuCategories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MenuCategory category)
        {
            var existing = await _context.MenuCategories
                .FirstOrDefaultAsync(c => c.Id == category.Id && !c.IsDeleted) 
                ?? throw new ApplicationException("Category not found");

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ApplicationException("Category name cannot be empty");

            if (!category.IsParentCategory && category.ParentCategoryId == null)
                throw new ApplicationException("Child category must have a parent");

            if (await ExistsByNameAsync(category.Name))
                throw new ApplicationException($"Category with name : {category.Name} exist");

            if (await ExistsByOrderNumberAsync(category.DisplayOrder, category.IsParentCategory))
                throw new ApplicationException($"Category with display order : " +
                    $"{category.DisplayOrder} exists");

            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.IsParentCategory = category.IsParentCategory;
            existing.ParentCategoryId = category.ParentCategoryId;
            existing.DisplayOrder = category.DisplayOrder;
            existing.IconPath = category.IconPath;
            existing.ModifiedAt = _dateService.Now;

            _context.MenuCategories.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var existing = await _context.MenuCategories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) 
                ?? throw new ApplicationException("Category not found");

            var hasChildren = await _context.MenuCategories.Where(e => e.ParentCategoryId == existing.Id)
                .AnyAsync();

            if (existing.IsParentCategory && hasChildren)
                throw new ApplicationException($"Category is a parent. Clear or Remove childs before " +
                    $"proceeding.");

            existing.IsDeleted = true;
            existing.ModifiedAt = _dateService.Now;

            _context.MenuCategories.Update(existing);
            await _context.SaveChangesAsync();
        }
    }
}
