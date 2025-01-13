namespace Application.Dtos
{
    public class TokensDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public int AccessTokenExpirationInSeconds { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public int RefreshTokenExpirationInSeconds { get; set; }
    }
}
