using Domain.Interfaces;
using Domain.Models.Users;
using Infrastructure.Data;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context, PasswordHasher hasher, JwtProvider jwtProvider) : IUserRepository
    {
        public async Task<User?> GetByIdAsync(UserId id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !hasher.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            string token = jwtProvider.CreateToken(user);

            return token;
        }

        public async Task<User?> RegisterAsync(string login, string password, string email, string firstName, string lastName)
        {
            if (await context.Users.AnyAsync(u => u.Email == email))
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            var passwordHash = hasher.Hash(password);

            User user = new User
            {
                Id = new UserId(new Guid()),
                Login = login,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PasswordHash = passwordHash
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }
    }
}
