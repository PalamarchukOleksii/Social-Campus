namespace Domain.Models.TokensModel
{
    public class Tokens(string accessToken, int accessTokenExpirationInMinutes, string refreshToken, int refreshTokenExpirationInDays)
    {
        public string AccessToken { get; private set; } = accessToken;
        public int AccessTokenExpirationInMinutes { get; private set; } = accessTokenExpirationInMinutes;
        public string RefreshToken { get; private set; } = refreshToken;
        public int RefreshTokenExpirationInDays { get; private set; } = refreshTokenExpirationInDays;
    }
}
