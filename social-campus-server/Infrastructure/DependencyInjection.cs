using Amazon.Runtime;
using Amazon.S3;
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
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HasherOptions>(configuration.GetSection("Hasher"));
        services.AddSingleton<IHasher, Hasher>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseAsyncSeeding(async (context, _, ct) => await DatabaseSeeder.SeedAsync(context, services, ct)));

        services.AddDataProtection()
            .PersistKeysToDbContext<ApplicationDbContext>()
            .SetApplicationName("SocialCampus");

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
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<StorageOptions>>().Value;

            var config = new AmazonS3Config
            {
                ForcePathStyle = true,
                ServiceURL = options.Endpoint,
            };

            var credentials = new BasicAWSCredentials(options.AccessKey, options.SecretKey);

            return new AmazonS3Client(credentials, config);
        });
        services.AddSingleton<IStorageService, StorageService>();

        services.Configure<EmailOptions>(configuration.GetSection("Email"));
        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IEmailLinkFactory, EmailLinkFactory>();
        services.AddHttpContextAccessor();

        return services;
    }
}