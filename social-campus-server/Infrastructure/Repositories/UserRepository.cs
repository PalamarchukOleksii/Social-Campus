using Domain.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsLoginUniqueAsync(string login)
        {
            return !await context.Users.AnyAsync(u => u.Login == login);
        }

        public async void AddAsync(User user)
        {
            await context.Users.AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByRefreshTokenIdAsync(int refreshTokenId)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.RefreshTokenId == refreshTokenId);
        }
    }
}
