using BlogApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace BlogApi.DataAcess
{
    public class ContextApi(DbContextOptions<ContextApi> options) : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
