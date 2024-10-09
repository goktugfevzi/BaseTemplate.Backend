using BaseTemplate.Schema.Entities;
using BaseTemplate.Shared.Dtos.TokenDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Abstractions.Token
{
    public interface ITokenHandler
    {
        TokenDto CreateAccessToken(User user );
        string CreateRefreshToken();
    }
}
