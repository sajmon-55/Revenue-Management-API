using Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class DatabaseSeeder(DatabaseContext dbContext, IConfiguration configuration)
{
    private readonly List<IDataSeeder> _seeders = [
        new AdminAccountSeeder()
    ];
    
    public async Task SeedDataAsync()
    {
        await dbContext.Database.MigrateAsync();

        foreach (var seeder in _seeders)
        {
            await seeder.SeedDataAsync(dbContext, configuration);
        }
    }
}