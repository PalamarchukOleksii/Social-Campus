using Domain.Models;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByLoginAsync(string login)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<User?> CreateUserAsync(string login, string passwordHash, string email, string firstName, string lastName)
        {
            if (await GetUserByEmailAsync(email) is not null || await GetUserByLoginAsync(login) is not null)
            {
                return default;
            }

            User user = new()
            {
                Login = login,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PasswordHash = passwordHash
            };

            context.Users.Add(user);

            return user;
        }
    }
}
