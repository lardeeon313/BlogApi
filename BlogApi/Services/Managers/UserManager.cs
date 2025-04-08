using BlogApi.Auth;
using BlogApi.Models.Dtos;
using BlogApi.Models.Entities;
using BlogApi.Repository.Interfaces;
using BlogApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace BlogApi.Services.Managers
{
    public class UserManager(IUserRepository userRepository, JwtHelper jwtHelper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly JwtHelper _jwtHelper = jwtHelper;

        public async Task<bool> RegisterUserAsync(UserDto userDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
                throw new Exception("User already exists.");

            var newUser = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = "User" // 👈 Agregar rol por defecto
            };

            return await _userRepository.RegisterUserAsync(newUser);
        }

        public async Task<AuthResponseDto?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role  // Asumiendo que el usuario tiene un campo "Role"
            };
        }
    }
}
