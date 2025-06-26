namespace Application.Abstractions.Security;

public interface IHasher
{
    Task<string> HashAsync(string value);
    Task<bool> VerifyAsync(string value, string valueHash);
}