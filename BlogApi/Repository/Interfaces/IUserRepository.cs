using BlogApi.Models.Entities;

namespace BlogApi.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> RegisterUserAsync(User user);
    }
}
