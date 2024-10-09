using AutoMapper;
using BaseTemplate.Business.Abstractions;
using BaseTemplate.Dal.Abstractions;
using BaseTemplate.Entity.Entities;
using BaseTemplate.Shared.Dtos.TokenDtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IConfiguration configuration;
        private readonly IUserRepository _userRepository;


        public UserService(IGenericRepository<User> genericRepository, IMapper mapper, IConfiguration configuration) : base(genericRepository, mapper)
        {
            this.configuration = configuration;
        }

        public async Task UpdateRefreshTokenAsync(User user, TokenDto token)
        {
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenEndDate = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["Token:LifeTime"]) + Convert.ToInt32(configuration["Token:RefreshTokenExtraTime"]));
            _userRepository.Update(user);
            await _userRepository.SaveAsync();
        }

    }
}
