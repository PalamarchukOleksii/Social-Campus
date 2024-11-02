using Domain.Models;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> CreateUserAsync(string login, string passwordHash, string email, string firstName, string lastName);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByLoginAsync(string login);
    }
}
