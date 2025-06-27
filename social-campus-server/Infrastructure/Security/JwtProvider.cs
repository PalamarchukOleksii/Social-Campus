using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Security;
using Application.Dtos;
using Domain.Models.UserModel;
using Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly string _secretKey = options.Value.SecretKey;
    private readonly string _issuer = options.Value.Issuer;
    private readonly string _audience = options.Value.Audience;
    private readonly int _accessTokenExpirationInSeconds = options.Value.AccessTokenExpirationInSeconds;
    private readonly int _refreshTokenExpirationInSeconds = options.Value.RefreshTokenExpirationInSeconds;

    public TokensDto GenerateTokens(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        return new TokensDto
        {
            AccessToken = accessToken,
            AccessTokenExpirationInSeconds = _accessTokenExpirationInSeconds,
            RefreshToken = refreshToken,
            RefreshTokenExpirationInSeconds = _refreshTokenExpirationInSeconds
        };
    }

    public async Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token)
    {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_secretKey));

        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = securityKey,
            ClockSkew = TimeSpan.Zero
        };

        JsonWebTokenHandler handler = new();
        var result = await handler.ValidateTokenAsync(token, tokenValidationParameters);

        if (!result.IsValid || result.ClaimsIdentity == null) throw new SecurityTokenException("Invalid token");

        return new ClaimsPrincipal(result.ClaimsIdentity);
    }

    private string GenerateAccessToken(User user)
    {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_secretKey));

        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ]),
            Expires = DateTime.UtcNow.AddSeconds(_accessTokenExpirationInSeconds),
            SigningCredentials = credentials,
            Issuer = _issuer,
            Audience = _audience
        };

        JsonWebTokenHandler handler = new();
        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[128];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}