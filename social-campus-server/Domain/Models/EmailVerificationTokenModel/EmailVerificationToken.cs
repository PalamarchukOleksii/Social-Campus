using Domain.Models.UserModel;

namespace Domain.Models.EmailVerificationTokenModel;

public class EmailVerificationToken
{
    private EmailVerificationToken()
    {
    }

    public EmailVerificationToken(UserId userId, int expirationInSeconds)
    {
        Id = new EmailVerificationTokenId(Guid.NewGuid());
        UserId = userId;
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddSeconds(expirationInSeconds);
    }

    public EmailVerificationTokenId Id { get; private set; } = new(Guid.Empty);
    public UserId UserId { get; private set; } = new(Guid.Empty);
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public virtual User? User { get; }
}