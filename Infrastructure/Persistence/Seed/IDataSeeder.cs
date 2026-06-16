using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Seed;

public interface IDataSeeder
{
    Task SeedDataAsync(DatabaseContext dbContext, IConfiguration configuration);
}