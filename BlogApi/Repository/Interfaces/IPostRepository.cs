using BlogApi.Models.Entities;

namespace BlogApi.Repository.Interfaces
{
    public interface IPostRepository
    {
        Task<bool> AddPostAsync(Post post);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task<bool> DeletePostAsync(int id);
        Task<bool> UpdatePostAsync(Post post, bool update);
    }
}
