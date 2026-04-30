using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SonLokma.Domain.Entities;
using SonLokma.Domain.Enums;

namespace SonLokma.Infrastructure.Persistence.Seed;

public static class DevelopmentDataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SonLokmaDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

        const string adminEmail = "admin@sonlokma.local";
        var normalizedEmail = adminEmail.ToUpperInvariant();
        var adminExists = await dbContext.Users.AnyAsync(user => user.NormalizedEmail == normalizedEmail);

        if (adminExists)
        {
            return;
        }

        var admin = new User
        {
            Id = Guid.NewGuid(),
            FullName = "SonLokma Admin",
            Email = adminEmail,
            NormalizedEmail = normalizedEmail,
            Role = UserRole.Admin,
            CreatedAt = DateTimeOffset.UtcNow
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin1234!");
        dbContext.Users.Add(admin);
        await dbContext.SaveChangesAsync();
    }
}
