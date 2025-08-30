using Domain.Entities.UserMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(long id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(long id);
        Task CreateUserPassAsync(long id, string userName, string password);
        Task UpdateUserPassAsync(long id, string password);
        Task DeleteUserPassAsync(long userId);
        Task<User?> ValidateUserAsync(string username, string password);
        Task ToggleLockUserAsync(long id);
        Task BlockUserAsync(long id, DateTime blockedUntil, string remarks);
        Task UnlockUserAsync(long id);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}
