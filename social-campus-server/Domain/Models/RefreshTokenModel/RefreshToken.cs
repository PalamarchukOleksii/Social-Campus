using Domain.Models.UserModel;

namespace Domain.Models.RefreshTokenModel;

public class RefreshToken
{
    private RefreshToken()
    {
    }

    public RefreshToken(string token, DateTime tokenExpiryDateTime, UserId userId)
    {
        Id = new RefreshTokenId(Guid.NewGuid());
        Token = token;
        TokenExpiryTime = tokenExpiryDateTime;
        UserId = userId;
    }

    public RefreshTokenId Id { get; private set; } = new(Guid.Empty);
    public string Token { get; private set; } = string.Empty;
    public DateTime TokenExpiryTime { get; private set; }
    public UserId UserId { get; private set; } = new(Guid.Empty);
    public virtual User? User { get; }

    public void UpdateTokenOnRefresh(string newToken, DateTime tokenExpiryDateTime)
    {
        Token = newToken;
        TokenExpiryTime = tokenExpiryDateTime;
    }

    public bool IsValid()
    {
        return DateTime.UtcNow < TokenExpiryTime;
    }

    public int SecondsUntilExpiration()
    {
        var remainingTime = TokenExpiryTime - DateTime.UtcNow;
        return remainingTime.Seconds;
    }
}