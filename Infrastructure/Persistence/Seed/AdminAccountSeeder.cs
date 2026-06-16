using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Seed;

public class AdminAccountSeeder : IDataSeeder
{
    public async Task SeedDataAsync(DatabaseContext dbContext, IConfiguration configuration)
    {
        if (await dbContext.Users.AnyAsync())
            return;
        
        var adminLogin = configuration["AdminAccount:Login"];
        adminLogin ??= "admin";
        
        var adminPassword = configuration["AdminAccount:Password"];
        adminPassword ??= "admin";
        
        var passwordHash = new PasswordHasher<object>().HashPassword(null!, adminPassword);

        var adminUser = new User
        {
            Login = adminLogin,
            PasswordHash = passwordHash,
            Role = Role.Admin
        };
        
        await dbContext.Users.AddAsync(adminUser);
        await dbContext.SaveChangesAsync();
    }
}