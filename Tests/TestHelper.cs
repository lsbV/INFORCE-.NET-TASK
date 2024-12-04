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

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
    public static OriginalUrl GetRandomUrl()
    {
        var guid = Guid.NewGuid().ToString();
        return new OriginalUrl($"https://www.example.com/{guid}");
    }

}
