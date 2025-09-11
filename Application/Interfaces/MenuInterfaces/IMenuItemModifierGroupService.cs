using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IMenuItemModifierGroupService
    {
        // Queries
        Task<List<MenuItemModifierGroup>> GetByMenuItemAsync(long menuItemId);
        Task<MenuItemModifierGroup> GetByIdAsync(long id);

        // Existence
        Task<bool> ExistsAsync(long menuItemId, long modifierGroupId, long? excludeId = null);

        // Commands
        Task<long> AddAsync(MenuItemModifierGroup link);
        Task DeleteAsync(long id);
    }
}
