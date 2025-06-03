using System.Text;
using Application.Abstractions.Data;
using Application.Abstractions.Security;
using Application.Abstractions.Storage;
using Domain.Abstractions.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringKey = "LocalConnection";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(connectionStringKey)));

        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                ClockSkew = TimeSpan.Zero
            });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<IPublicationRepository, PublicationRepository>();
        services.AddScoped<IPublicationLikeRepositories, PublicationLikeRepositories>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddSingleton<IStorageService, MinioStorageService>();

        return services;
    }
}