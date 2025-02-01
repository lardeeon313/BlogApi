using BlogApi.DataAcess;
using BlogApi.Models.Entities;
using BlogApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repository.Repositories
{
    public class PostRepository(ContextApi context) : IPostRepository
    {
        private readonly ContextApi _context = context;    


        public async Task<bool> AddPostAsync(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));

            await _context.Posts.AddAsync(post);
            var result = await _context.SaveChangesAsync();
            return result > 0; 
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _context.Set<Post>().FindAsync(id);

            if (post != null)
            {
                _context.Set<Post>().Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;

        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.ToListAsync();

        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        
    }
}
