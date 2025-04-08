using BlogApi.Models.Dtos;

namespace BlogApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserDto userDto);
        Task<AuthResponseDto> AuthenticateUserAsync(string email, string password);
    }
}
