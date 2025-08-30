using Domain.Entities.UserMod;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.UserInterfaces
{
    public interface IPermissionService
    {
        Task<List<Permission>> GetAllAsync();
        Task<List<Permission>> GetByPermissionGroupAsync(long permissionId);
        Task<Permission> GetByIdAsync(long id);
        Task AddAsync(Permission permission);
        Task UpdateAsync(Permission permission);
        Task DeleteAsync(long id);
        Task<bool> ExistsAsync(long permissionGroupId, string action);
        Task<List<PermissionActionEnum>> GetAvailableActionsAsync();
    }
}
