using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IMenuCategoryService
    {
        Task<List<MenuCategory>> GetAllAsync();
        Task<MenuCategory?> GetByIdAsync(long id);
        Task<List<MenuCategory>> GetParentCategoriesAsync(); // IsParentCategory == true
        Task<List<MenuCategory>> GetChildCategoriesAsync(long parentId); // For nested structure

        Task AddAsync(MenuCategory category);
        Task UpdateAsync(MenuCategory category);
        Task DeleteAsync(long id);

        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByOrderNumberAsync(int orderNo, bool isParentCategory);
        Task<bool> HasChildCategoriesAsync(long parentId);
    }
}
