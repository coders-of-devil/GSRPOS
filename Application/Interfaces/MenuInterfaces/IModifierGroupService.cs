using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IModifierGroupService
    {
        // Queries
        Task<List<ModifierGroup>> GetAllAsync();
        Task<ModifierGroup> GetByIdAsync(long id);

        // Existence
        Task<bool> ExistsByNameAsync(string name, long? excludeId = null);

        // Commands
        Task<long> AddAsync(ModifierGroup group);
        Task UpdateAsync(ModifierGroup group);
        Task DeleteAsync(long id);

    }
}
