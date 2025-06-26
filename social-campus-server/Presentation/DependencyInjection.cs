using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Presentation.Extensions;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorPages();
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

        services.AddEndpoints(Assembly.GetExecutingAssembly());
        services.AddClientCors(
            "ClientCors",
            [
                "http://localhost:3000",
                "https://localhost:3000"
            ]
        );

        return services;
    }
}