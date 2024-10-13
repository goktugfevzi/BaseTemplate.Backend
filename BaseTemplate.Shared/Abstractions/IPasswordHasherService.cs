using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Abstractions
{
    public interface IPasswordHasherService
    {
        public bool VerifyPassword(string password, byte[] hashedPassword);
        public byte[] HashPassword(string password);
    }
}
