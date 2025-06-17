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
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("LocalConnection"))
                .UseAsyncSeeding(async (context, _, ct) => await DatabaseSeeder.SeedAsync(context, services, ct)));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<IPublicationRepository, PublicationRepository>();
        services.AddScoped<IPublicationLikeRepository, PublicationLikeRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IPublicationTagRepository, PublicationTagRepository>();

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddSingleton<IStorageService, MinioStorageService>();

        return services;
    }
}