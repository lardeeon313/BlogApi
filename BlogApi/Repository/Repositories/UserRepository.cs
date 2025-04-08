using BlogApi.DataAcess;
using BlogApi.Models.Entities;
using BlogApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogApi.Repository.Repositories
{
    public class UserRepository(ContextApi context) : IUserRepository
    {
        private readonly ContextApi _context = context;

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
