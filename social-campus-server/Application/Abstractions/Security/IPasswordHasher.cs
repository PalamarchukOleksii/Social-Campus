namespace Application.Abstractions.Security
{
    public interface IPasswordHasher
    {
        Task<string> HashAsync(string password);
        Task<bool> VerifyAsync(string password, string passwordHash);
    }
}
