using System;

namespace Presentation.Extensions;

public static class CorsExtension
{
    public static IServiceCollection AddClientCors(this IServiceCollection services, string policyName, string[] allowedOrigins)
    {
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
}
