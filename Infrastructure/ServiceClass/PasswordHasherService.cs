using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceClass
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password, string salt, string storedHash)
        {
            var hash = HashPassword(password, salt);
            return hash == storedHash;
        }
    }
}
