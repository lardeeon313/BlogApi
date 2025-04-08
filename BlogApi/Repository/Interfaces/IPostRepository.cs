using BlogApi.Models.Entities;
using BlogApi.Models.Enums;

namespace BlogApi.Repository.Interfaces
{
    public interface IPostRepository
    {
        Task AddPostAsync(Post post);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task DeletePostAsync(int id);
        Task UpdatePostAsync(Post post);
        Task<IEnumerable<Post>> FilterPostsAsync(string? title, Category? category, string? tags, string? content);

    }
}
