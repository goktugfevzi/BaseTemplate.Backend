using BaseTemplate.Domain.Entities;
using BaseTemplate.Shared.Dtos.LoginDtos;
using BaseTemplate.Shared.Dtos.SystemDtos;
using BaseTemplate.Shared.Dtos.TokenDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Abstractions
{
    public interface IUserService : IGenericService<User>
    {
        Task UpdateRefreshTokenAsync(User user, TokenDto token);
        Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto loginDto);
        Task<ServiceResult<User>> RegisterAsync(RegisterUserDto registerUserDto);
        Task<ServiceResult<LoginResponseDto>> LoginWithRefreshTokenAsync(string refreshToken);

    }
}
