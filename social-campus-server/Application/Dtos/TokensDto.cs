namespace Application.Dtos
{
    public class TokensDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public int AccessTokenExpirationInMinutes { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public int RefreshTokenExpirationInDays { get; set; }
    }
}
