using BaseTemplate.Shared.Dtos.LoginDtos;
using BaseTemplate.Shared.Dtos.SystemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Abstractions.Token
{
    public interface IAuthService
    {
        Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto loginDto);
        Task<ServiceResult<LoginResponseDto>> LoginWithRefreshTokenAsync(string refreshToken);
    }
}
