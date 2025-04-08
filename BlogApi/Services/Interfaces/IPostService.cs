using BlogApi.Models.Dtos;
using BlogApi.Models.Entities;

namespace BlogApi.Services.Interfaces
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(PostDto postDto, int userId);
        Task<(IEnumerable<PostDto> Posts, int TotalCount)> GetAllPostsAsync(int page, int pageSize);
        Task<PostDto> GetPostByIdAsync(int id);
        Task<bool> DeletePostByIdAsync(int id, int userId);
        Task<bool> UpdatePostAsync(int id, PostDto postDto, int userId);
        Task<IEnumerable<PostDto>> FilterPostsAsync(string? title, string? category, string? tags, string? content);


    }
}
