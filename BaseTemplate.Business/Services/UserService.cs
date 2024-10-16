using AutoMapper;
using BaseTemplate.Business.Abstractions;
using BaseTemplate.Business.Abstractions.Token;
using BaseTemplate.Domain.Entities;
using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Repository.Conrete;
using BaseTemplate.Shared.Abstractions;
using BaseTemplate.Shared.Dtos.LoginDtos;
using BaseTemplate.Shared.Dtos.SystemDtos;
using BaseTemplate.Shared.Dtos.TokenDtos;
using Microsoft.Extensions.Configuration;

namespace BaseTemplate.Business.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly ITokenHandler _tokenHandler;

        public UserService(IGenericRepository<User> genericRepository, IMapper mapper, IConfiguration configuration, ITokenHandler tokenHandler, IPasswordHasherService passwordHasher, IUserRepository userRepository) : base(genericRepository, mapper)
        {
            _configuration = configuration;
            _tokenHandler = tokenHandler;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }


        public async Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto loginDto)
        {
            var user = (await _userRepository.GetSingleAsync(u => u.Username == loginDto.Username));

            if (user == null || _passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return ServiceResult<LoginResponseDto>.Fail(400, "Invalid email or password");
            }

            var token = _tokenHandler.CreateAccessToken(user);

            return ServiceResult<LoginResponseDto>.Success(200, new LoginResponseDto
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            });
        }



        public async Task<ServiceResult<User>> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var user = (await _userRepository.GetSingleAsync(u => u.Username == registerUserDto.Username));


            if (user != null)
            {
                return ServiceResult<User>.Fail(400, "Kullanıcı zaten kayıtlı");
            }

            user = new User
            {
                Name = registerUserDto.Name,
                Surname = registerUserDto.Surname,
                Username = registerUserDto.Username,
                Email = registerUserDto.Email,
                PasswordHash = _passwordHasher.HashPassword(registerUserDto.Password)
            };


            var refreshToken = _tokenHandler.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Token:LifeTime"]) + Convert.ToInt32(_configuration["Token:RefreshTokenExtraTime"]));

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
            return ServiceResult<User>.Success(201, user);
        }

        public async Task<ServiceResult<LoginResponseDto>> LoginWithRefreshTokenAsync(string refreshToken)
        {
            var user = (await _userRepository.GetSingleAsync(u => u.RefreshToken == refreshToken));

            if (user == null)
            {
                return ServiceResult<LoginResponseDto>.Fail(400, "Geçersiz Refresh Token");
            }

            if (user.RefreshTokenEndDate == null || user.RefreshTokenEndDate <= DateTime.UtcNow)
            {
                return ServiceResult<LoginResponseDto>.Fail(400, "Refresh Token süresi dolmuş");
            }

            var token = _tokenHandler.CreateAccessToken(user);
            await UpdateRefreshTokenAsync(user, token);

            return ServiceResult<LoginResponseDto>.Success(200, new LoginResponseDto
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Username = user.Username
            });
        }

        public async Task UpdateRefreshTokenAsync(User user, TokenDto token)
        {
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenEndDate = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Token:LifeTime"]) + Convert.ToInt32(_configuration["Token:RefreshTokenExtraTime"]));
            _userRepository.Update(user);
            await _userRepository.SaveAsync();
        }

    }
}
