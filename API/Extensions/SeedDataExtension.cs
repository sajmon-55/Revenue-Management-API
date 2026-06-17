using Infrastructure.Persistence;

namespace API.Extensions;

public static class SeedDataExtension
{
    public static async Task SeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedDataAsync();
    }
}