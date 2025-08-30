using Domain.Entities.Common;
using Domain.Entities.MenuMod;
using Domain.Entities.UserMod;
using Domain.Services;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seed
{
    public class RuntimeSeeder(AppDbContext context, IIdGeneratorService idGeneratorService,
        IPasswordHasherService passwordHasherService, IDateService dateService)
    {
        private readonly AppDbContext _context = context;
        private readonly IIdGeneratorService _idGeneratorService = idGeneratorService;
        private readonly IPasswordHasherService _passwordHasherService = passwordHasherService;
        private readonly IDateService _dateService = dateService;

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Roles.Any())
            {
                var superAdminRole = new Role
                {
                    Id = await _idGeneratorService.GenerateNextId<Role>(),
                    Name = "Super Admin",
                    IsSystemRole = true,
                    IsPermissionAssigned = true,
                    IsActive = true
                };
                _context.Roles.Add(superAdminRole);
                await _context.SaveChangesAsync();

                var adminRole = new Role
                {
                    Id = await _idGeneratorService.GenerateNextId<Role>(),
                    Name = "Admin",
                    IsSystemRole = false,
                    IsActive = true
                };

                _context.Roles.Add( adminRole);
                await _context.SaveChangesAsync();
            }

            if (!_context.Users.Any())
            {
                var salt = _passwordHasherService.GenerateSalt();

                var superAdminUser = new User
                {
                    Id = await _idGeneratorService.GenerateNextId<User>(),
                    Fullname = "Super Admin",
                    Email = "sadmin@gsrpos.com",
                    PhoneNo = "052xxxxxxx",
                    Username = "sadmin001",
                    Password = _passwordHasherService.HashPassword("12345", salt),
                    Saltkey = salt,
                    Hint = "Adm Com",
                    IsActive = true
                };

                _context.Users.Add(superAdminUser);
                await _context.SaveChangesAsync();
            }

            if (!_context.UsersPass.Any())
            {
                var userId = _context.Users.First(u => u.Fullname == "Super Admin").Id;

                var superAdminUserPass = new UserPass
                {
                    Id = await _idGeneratorService.GenerateNextId<UserPass>(),
                    UserId = userId,
                    UsersPass = "12345"
                };

                _context.UsersPass.Add(superAdminUserPass);
                await _context.SaveChangesAsync();
            }

            if (!_context.UserRoles.Any())
            {
                var userId = _context.Users.First(u => u.Fullname == "Super Admin").Id;
                var roleId = _context.Roles.First(r => r.Name == "Super Admin").Id;

                var superAdminUserRole = new UserRole
                {
                    Id = await _idGeneratorService.GenerateNextId<UserRole>(),
                    UserId = userId,
                    RoleId = roleId,
                    Description = "Super Admin of the system",
                    IsActive = true
                };

                _context.UserRoles.Add(superAdminUserRole);
                await _context.SaveChangesAsync();
            }

            if (!_context.Settings.Any())
            {
                var settings = new Setting
                {
                    Id = 1,
                    IsTaxable = false,
                    IsTaxExclusive = false,
                    IsTaxInclusive = false,
                    TaxRate = 5,
                    IsMultiplePriceForItem = false
                };

                _context.Settings.Add(settings);
                await _context.SaveChangesAsync();
            }

            if (!_context.PricelistTypes.Any())
            {
                var defaultType = new PricelistTypes
                {
                    Id = 1,
                    Name = "Default Type",
                    Description = "Default Pricelist Type"
                };

                var systemType = new PricelistTypes
                {
                    Id = 2,
                    Name = "System Type",
                    Description = "System Pricelist Type"
                };

                var festiveType = new PricelistTypes
                {
                    Id = 3,
                    Name = "Festive Type",
                    Description = "Festive Pricelist Type"
                };

                _context.PricelistTypes.Add(defaultType);
                _context.PricelistTypes.Add(systemType);
                _context.PricelistTypes.Add(festiveType);

                await _context.SaveChangesAsync();
            }

            if (!_context.PriceLists.Any())
            {
                var defaultPricelist = new PriceList
                {
                    Id = 1,
                    TypeId = 1,
                    Name = "Default Pricelist",
                    IsActive = true,
                    IsTemporary = false
                };

                _context.PriceLists.Add(defaultPricelist);
                await _context.SaveChangesAsync();
            }
        }
    }
}
