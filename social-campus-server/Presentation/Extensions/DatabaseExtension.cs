using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extensions;

public static class DatabaseExtension
{    
    public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app, IServiceCollection services,
        CancellationToken cancellationToken = default)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await context.Database.MigrateAsync(cancellationToken);

        await DatabaseSeeder.SeedAsync(context, services, cancellationToken);
    }
}