using Domain.Models.Users;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(UserId id);
        Task<User?> RegisterAsync(string login, string password, string email, string firstName, string lastName);
        Task<string> LoginAsync(string email, string password);
    }
}
