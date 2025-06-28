namespace Presentation.Extensions;

public static class CorsExtension
{
    public static IServiceCollection AddClientCors(this IServiceCollection services, string policyName,
        string allowedOrigin)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.WithOrigins(allowedOrigin)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}