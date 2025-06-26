using Presentation.Urls;

namespace Presentation.Extensions;

public static class CorsExtension
{
    private static IServiceCollection AddClientCors(this IServiceCollection services, string policyName,
        string[] allowedOrigins)
    {
        foreach (var origin in allowedOrigins)
            if (!Uri.TryCreate(origin, UriKind.Absolute, out _))
                throw new InvalidOperationException($"Invalid CORS origin URL: {origin}");

        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddClientCorsFromConfiguration(this IServiceCollection services,
        IConfiguration configuration, string policyName = "ClientCors")
    {
        var corsOrigins = configuration
            .GetSection($"{ApplicationUrlsOptions.SectionName}:Cors:AllowedOrigins")
            .Get<string[]>();

        if (corsOrigins == null || corsOrigins.Length == 0)
            throw new InvalidOperationException(
                "CORS origins must be configured in ApplicationUrls:Cors:AllowedOrigins");

        return services.AddClientCors(policyName, corsOrigins);
    }
}