using Domain.Entities.UserMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.UserInterfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(long id);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(long id);
    }
}
