using Infrastructure.Data;

namespace Presentation.Extensions;

public static class DatabaseExtension
{
    public static async Task EnsureDatabaseCreatedAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
    }
}