using Domain.Models.UserModel;

namespace Domain.Models.EmailVerificationTokenModel;

public class EmailVerificationToken
{
    private EmailVerificationToken()
    {
    }

    public EmailVerificationToken(string email, string tokenHash, int expirationInSeconds)
    {
        Id = new EmailVerificationTokenId(Guid.NewGuid());
        Email = email;
        TokenHash = tokenHash;
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddSeconds(expirationInSeconds);
    }

    public EmailVerificationTokenId Id { get; private set; } = new(Guid.Empty);
    public string Email { get; private set; } = string.Empty;
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
}