using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IPasswordHasherService
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
        bool VerifyPassword(string password, string salt, string storedHash);
    }
}
