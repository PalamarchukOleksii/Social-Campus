﻿using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Security;
using Application.Dtos;
using Domain.Models.UserModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class JwtProvider(IConfiguration configuration) : IJwtProvider
{
    public TokensDto GenerateTokens(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        return new TokensDto
        {
            AccessToken = accessToken,
            AccessTokenExpirationInSeconds = configuration.GetValue<int>("Jwt:AccessTokenExpirationInSeconds"),
            RefreshToken = refreshToken,
            RefreshTokenExpirationInSeconds = configuration.GetValue<int>("Jwt:RefreshTokenExpirationInSeconds")
        };
    }

    public async Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token)
    {
        var secretKey = configuration["Jwt:SecretKey"]!;
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));

        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
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
        var secretKey = configuration["Jwt:SecretKey"]!;
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));

        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ]),
            Expires = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("Jwt:AccessTokenExpirationInSeconds")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
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