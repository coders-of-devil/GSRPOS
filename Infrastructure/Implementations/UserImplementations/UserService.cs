using Applications.Interfaces.UserInterfaces;
using Domain.Entities.UserMod;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.UserImplementations
{
    public class UserService(AppDbContext context, IIdGeneratorService idGenerator, IDateService dateService,
        IPasswordHasherService passwordHasherService) : IUserService
    {
        private readonly AppDbContext _context = context;
        private readonly IDateService _dateService = dateService;
        private readonly IIdGeneratorService _idGenerator = idGenerator;
        private readonly IPasswordHasherService _passwordHasher = passwordHasherService;

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.Fullname)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            if (id == 0)
                throw new ApplicationException("User id is empty.");

            var user = await _context.Users.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (user == null)
                throw new ApplicationException($"user with id : {id} not found");

            return user;
        }

        public async Task CreateUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Fullname))
                throw new ApplicationException($"Name of user is empty.");
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ApplicationException($"Email of user is empty.");

            var exists = await _context.Users.AnyAsync(r => r.Fullname == user.Fullname && !r.IsDeleted);
            if (exists)
                throw new ApplicationException($"User with email : {user.Email} already exists");

            var salt = _passwordHasher.GenerateSalt();

            user.Id = await _idGenerator.GenerateNextId<User>();
            user.Password = _passwordHasher.HashPassword(user.Password, salt);
            user.Saltkey = salt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userPass = new UserPass()
            {
                Id = await _idGenerator.GenerateNextId<UserPass>(),
                UserId = user.Id,
                UsersPass = user.Password
            };

            _context.UsersPass.Add(userPass);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(e => e.IsDeleted == false 
                && e.Id == user.Id);
            if (existingUser == null)
                throw new ApplicationException($"User not found.");

            if (string.IsNullOrWhiteSpace(user.Fullname))
                throw new ApplicationException($"Name of user is empty.");
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ApplicationException($"Email of user is empty.");

            var exists = await _context.Users.AnyAsync(r => r.Fullname == user.Fullname && !r.IsDeleted);
            if (exists)
                throw new ApplicationException($"User with email : {user.Email} already exists");

            existingUser.Fullname = user.Fullname;
            existingUser.PhoneNo = user.PhoneNo;
            existingUser.Email = user.Email;
            existingUser.ModifiedAt = _dateService.Now;

            _context.Users.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(long id)
        {
            if (id == 0)
                throw new ApplicationException($"User id is empty.");

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
                throw new ApplicationException($"user with id : {id} not found");

            user.IsDeleted = true;
            user.IsActive = false;
            user.ModifiedAt = _dateService.Now;

            _context.Users.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CreateUserPassAsync(long id, string username, string password)
        {
            if (id == 0) throw new ApplicationException($"User id is empty.");

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
                throw new ApplicationException($"User not found.");

            user.Username = username;
            user.Saltkey = _passwordHasher.GenerateSalt();
            user.Password = _passwordHasher.HashPassword(password, user.Saltkey);
            user.ModifiedAt = _dateService.Now;

            var userPass = await _context.UsersPass.FirstOrDefaultAsync(e => e.UserId == id);
            if(userPass == null)
            {
                var pass = new UserPass()
                {
                    Id = await _idGenerator.GenerateNextId<UserPass>(),
                    UserId = id,
                    UsersPass = password
                };

                _context.UsersPass.Add(pass);
                await _context.SaveChangesAsync();
            }

            userPass.UsersPass = password;
            userPass.ModifiedAt = _dateService.Now;

            _context.UsersPass.Entry(userPass).State = EntityState.Modified;
            _context.Users.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserPassAsync(long id, string password)
        {
            if (id == 0) throw new ApplicationException($"User id is empty.");

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
                throw new ApplicationException($"User not found.");

            user.Saltkey = _passwordHasher.GenerateSalt();
            user.Password = _passwordHasher.HashPassword(password, user.Saltkey);
            user.ModifiedAt = _dateService.Now;

            var userPass = await _context.UsersPass.FirstOrDefaultAsync(e => e.UserId == id);

            userPass.UsersPass = password;
            userPass.ModifiedAt = _dateService.Now;

            _context.UsersPass.Entry(userPass).State = EntityState.Modified;
            _context.Users.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserPassAsync(long id)
        {
            if (id == 0) throw new ApplicationException($"User id is empty.");

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
                throw new ApplicationException($"User not found.");

            user.Password = "";
            user.Saltkey = "";
            user.Hint = "";
            user.ModifiedAt = _dateService.Now;

            var userPass = await _context.UsersPass.FirstOrDefaultAsync(e => e.UserId == id);

            userPass.UsersPass = "";
            userPass.ModifiedAt = _dateService.Now;

            _context.UsersPass.Entry(userPass).State = EntityState.Modified;
            _context.Users.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) throw new ApplicationException($"User not found.");

            var hashed = _passwordHasher.HashPassword(password, user.Saltkey);
            return user.Password == hashed ? user : null;
        }

        public async Task ToggleLockUserAsync(long id)
        {
            if (id == 0) throw new ApplicationException($"User id is empty.");

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
                throw new ApplicationException($"User not found.");

            if (user.IsLocked == true)
                user.IsLocked = false;
            else
                user.IsLocked = false;

            _context.Users.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task BlockUserAsync(long id, DateTime blockedUntil, string remarks)
        {
            if (id == 0) throw new ApplicationException($"User id is empty.");

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
                throw new ApplicationException($"User not found.");

            user.IsActive = false;
            user.IsLocked = true;
            user.BlockedUntil = blockedUntil;
            user.Remarks = remarks;
            user.ModifiedAt = _dateService.Now;

            _context.Users.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UnlockUserAsync(long id)
        {
            if (id == 0) throw new ApplicationException($"User id is empty.");

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
                throw new ApplicationException($"User not found.");

            user.IsActive = true;
            user.IsLocked = false;
            user.BlockedUntil = DateTime.MinValue;
            user.Remarks = "";
            user.ModifiedAt = _dateService.Now;

            _context.Users.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username && !u.IsDeleted);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && !u.IsDeleted);
        }
    }
}
