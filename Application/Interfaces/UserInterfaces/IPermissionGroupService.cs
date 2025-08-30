using Domain.Entities.UserMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.UserInterfaces
{
    public interface IPermissionGroupService
    {
        Task<List<PermissionGroup>> GetAllAsync();
        Task<PermissionGroup> GetByIdAsync(long id);
        Task AddAsync(PermissionGroup permissionGroup);
        Task UpdateAsync(PermissionGroup permissionGroup);
        Task DeleteAsync(long id);
    }
}
