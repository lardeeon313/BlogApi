using BlogApi.DataAcess;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using BlogApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repository.Repositories
{
    public class PostRepository(ContextApi context) : IPostRepository
    {
        private readonly ContextApi _context = context;    


        public async Task AddPostAsync(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));

            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Set<Post>().FindAsync(id);

            if (post == null)
                throw new KeyNotFoundException($"Post with ID {id} not found."); // 🔹 Lanzamos una excepción adecuada

            _context.Set<Post>().Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> FilterPostsAsync(string? title, Category? category, string? tags, string? content)
        {
            var query = _context.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(p => p.Title.Contains(title));

            if (category.HasValue)  // 🔹 Si tiene un valor, filtramos por el enum
                query = query.Where(p => p.Category == category.Value);

            if (!string.IsNullOrEmpty(tags))
                query = query.Where(p => p.Tags.Contains(tags));

            if (!string.IsNullOrEmpty(content))
                query = query.Where(p => p.Content.Contains(content));

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.AsNoTracking().ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdatePostAsync(Post post)
        {
            post.UpdatedAt = DateTime.UtcNow;

            _context.Set<Post>().Update(post);
            await _context.SaveChangesAsync();
        }
    }
}
