namespace Infrastructure.Security
{
    public class JwtOptions
    {
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required string SecretKey { get; init; }
        public required int ExpirationInMinutes { get; init; }
    }
}
