using BaseTemplate.Schema.Entities;
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

    }
}
