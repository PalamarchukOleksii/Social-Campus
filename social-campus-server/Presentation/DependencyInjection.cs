using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Presentation.Extensions;
using Presentation.Options;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
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
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                ClockSkew = TimeSpan.Zero
            });

        services.AddOpenApi(options => { options.AddDocumentTransformer<OpenApiExtension>(); });

        services.Configure<ApplicationUrlsOptions>(
            configuration.GetSection(ApplicationUrlsOptions.SectionName));
        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<ApplicationUrlsOptions>>().Value);

        services.AddEndpoints(Assembly.GetExecutingAssembly());
        services.AddClientCors("ClientCors", configuration.GetValue<string>("ApplicationUrls:ClientBaseUrl")!);

        return services;
    }
}