using Core;
using Database;
using Microsoft.EntityFrameworkCore;
using ShortenerComponent.Options;

namespace Tests;

internal static class TestHelper
{
    public static ShortenerServiceOptions GetShortenerServiceOptions()
    {
        return new ShortenerServiceOptions
        {
            HashLength = 6,
            MaxAttempts = 5,
            AllowedCharacters = "ABCDEFGHJKMNPQRSTUVWXYZ23456789",
            DefaultExpirationTime = TimeSpan.FromDays(7)
        };
    }

    public static ApplicationDbContext GetDbContext()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        var context = new TestDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
    public static OriginalUrl GetRandomUrl()
    {
        var guid = Guid.NewGuid().ToString();
        return new OriginalUrl($"https://www.example.com/{guid}");
    }

}

internal class TestDbContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().HasData(new User
        {
            Id = new UserId(1),
            UserName = "Test",
            Email = "test@email.com",
            PasswordHash = "password",
        });
        builder.Entity<Url>().HasData(new Url(
            new OriginalUrl("https://www.test.com"),
            new UrlHash("ABCDEF"),
            new UrlExpiration(DateTime.Now + TimeSpan.FromDays(7)),
            new UrlVisits(2),
            new UserId(1),
            new UrlCreationTime(DateTime.Now)));

        builder.Entity<Role>().HasData(
            new Role { Id = new UserId(1), Name = "Admin" },
            new Role { Id = new UserId(2), Name = "User" }
            );
    }
}