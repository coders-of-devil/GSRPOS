using Domain.Entities.MenuMod;

namespace Applications.Interfaces.MenuInterfaces
{
    public interface IMenuItemService
    {
        // Queries
        Task<List<MenuItem>> GetAllAsync();
        Task<MenuItem> GetByIdAsync(long id);
        Task<List<MenuItem>> GetByCategoryAsync(long categoryId);
        Task<List<MenuItem>> GetByPreparationAreaAsync(long preparationAreaId);

        // Existence
        Task<bool> ExistByNameAsync(string name, long categoryId, long? excludeId = null);
        Task<bool> ExistByOrderNoAsync(int orderNo, long categoryId, long? excludeId = null);

        // Commands
        Task<long> AddAsync(MenuItem item);
        Task UpdateAsync(MenuItem item);
        Task DeleteAsync(long id);
        Task MarkNotAvailableTodayAsync(long id);

        // Optional toggles
        Task SetActiveAsync(long id, bool isActive);
    }
}

