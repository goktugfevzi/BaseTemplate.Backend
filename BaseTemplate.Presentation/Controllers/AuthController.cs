using BaseTemplate.Business.Abstractions;
using BaseTemplate.Business.Abstractions.Token;
using BaseTemplate.Presentation.Attributes;
using BaseTemplate.Presentation.Controllers.Common;
using BaseTemplate.Shared.Dtos.LoginDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseTemplate.Presentation.Controllers
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [NoNeedAuthorization]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            return CreateActionResult(await _userService.RegisterAsync(registerUserDto));

        }
        [HttpPost]
        [NoNeedAuthorization]
        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {

            return CreateActionResult(await _userService.LoginAsync(loginDto));

        }
        [HttpPost]
        [NoNeedAuthorization]
        public async Task<IActionResult> LoginWithRefreshToken(string refreshToken)
        {
            return CreateActionResult(await _userService.LoginWithRefreshTokenAsync(refreshToken));

        }
    }
}
