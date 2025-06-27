using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Application.Abstractions.Security;
using Application.Abstractions.Storage;
using Domain.Abstractions.Repositories;
using Infrastructure.Data;
using Infrastructure.Email;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HasherOptions>(configuration.GetSection("Hasher"));
        services.AddSingleton<IHasher, Hasher>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        //.UseAsyncSeeding(async (context, _, ct) => await DatabaseSeeder.SeedAsync(context, services, ct)));

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

        services.Configure<EmailVerificationTokenOptions>(configuration.GetSection("EmailVerificationToken"));
        services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();

        services.Configure<ResetPasswordTokenOptions>(configuration.GetSection("ResetPasswordToken"));
        services.AddScoped<IResetPasswordTokenRepository, ResetPasswordTokenRepository>();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.Configure<StorageOptions>(configuration.GetSection("Storage"));
        services.AddSingleton<IStorageService, StorageService>();

        services.Configure<EmailOptions>(configuration.GetSection("Email"));
        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IEmailLinkFactory, EmailLinkFactory>();
        services.AddHttpContextAccessor();

        return services;
    }
}