using BaseTemplate.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public byte[] HashPassword(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPassword(string password, byte[] hashedPassword)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(hashedPassword);
        }
    }
}
