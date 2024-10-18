using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Dtos.LoginDtos
{
    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; } = "";
    }
}
