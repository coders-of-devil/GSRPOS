using Domain.DTOs;
using Domain.Entities.UserMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.UserInterfaces
{
    public interface IRolePermissionService
    {
        Task<List<RolePermission>> GetAllByRole(long r_Id);
        Task<List<RolePermission>> GetAllByPermission(long p_Id);
        Task<RolePermission> GetById(long id);
        Task CreateRolePermissionAsync(RolePermission rp);
        Task UpdateRolePermissionAsync(RolePermission rp);
        Task DeleteRolePermissionAsync(long id);
        Task<bool> ExistsAsync(long r_Id, long p_Id);
        Task RemoveAllPermissionsFromRoleAsync(long r_Id);
        Task AssignPermissionsToRoleAsync(long r_Id, List<long> p_Ids);
        Task<List<RolePermissionAssignmentDTO>> GetAssignablePermissionsAsync(long roleId);

    }
}
