using Domain.Models.UserModel;

namespace Domain.Models.ResetPasswordTokenModel;

public class ResetPasswordToken
{
    private ResetPasswordToken()
    {
    }

    public ResetPasswordToken(UserId userId, int expirationInSeconds)
    {
        Id = new ResetPasswordTokenId(Guid.NewGuid());
        UserId = userId;
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddSeconds(expirationInSeconds);
    }

    public ResetPasswordTokenId Id { get; private set; } = new(Guid.Empty);
    public UserId UserId { get; private set; } = new(Guid.Empty);
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public virtual User? User { get; }
}