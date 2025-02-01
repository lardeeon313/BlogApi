using BlogApi.Models.Dtos;
using BlogApi.Models.Entities;

namespace BlogApi.Services.Interfaces
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(PostDto postDto);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task<bool> DeletePostById(int id);

    }
}
