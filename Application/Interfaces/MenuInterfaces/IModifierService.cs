using Domain.Entities.MenuMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IModifierService
    {
        // Queries
        Task<List<Modifier>> GetByGroupAsync(long modifierGroupId);
        Task<Modifier> GetByIdAsync(long id);
        Task<List<Modifier>> GetAllAsync();

        // Existence (unique name per group)
        Task<bool> ExistsByNameAsync(string name, long modifierGroupId, long? excludeId = null);

        // Commands
        Task<long> AddAsync(Modifier modifier);
        Task UpdateAsync(Modifier modifier);
        Task DeleteAsync(long id);
    }
}
