using BlogApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.DataAcess
{
    public class ContextApi : DbContext
    {
        public ContextApi(DbContextOptions<ContextApi> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación 1:N entre User y Post
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina un usuario, se eliminan sus posts

            base.OnModelCreating(modelBuilder);
        }
    }
}
