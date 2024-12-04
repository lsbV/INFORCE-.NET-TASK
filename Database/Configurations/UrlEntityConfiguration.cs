using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

internal class UrlEntityConfiguration : IEntityTypeConfiguration<Url>
{
    public void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.HasKey(e => e.Hash);

        builder.Property(e => e.Hash)
            .HasConversion(v => v.Value, v => new UrlHash(v))
            .IsRequired();

        builder.Property(e => e.OriginalUrl)
            .HasConversion(v => v.Value, v => new OriginalUrl(v))
            .IsRequired();
        builder.HasIndex(e => e.OriginalUrl).IsUnique();

        builder.Property(e => e.Expiration)
            .HasConversion(v => v.Value, v => new UrlExpiration(v))
            .IsRequired();

        builder.Property(e => e.Visits)
            .HasConversion(v => v.Value, v => new UrlVisits(v))
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .HasConversion(v => v.Value, v => new UserId(v))
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasConversion(v => v.Value, v => new UrlCreationTime(v))
            .IsRequired();

        List<Url> urls =
        [
            new Url(
                new OriginalUrl("https://www.google.com"),
                new UrlHash("ASDFGH"),
                new UrlExpiration(DateTime.UtcNow.Date.AddDays(7)),
                new UrlVisits(3462),
                new UserId(1),
                new UrlCreationTime(DateTime.UtcNow.Date - TimeSpan.FromDays(1))
            ),
            new Url(
                new OriginalUrl("https://www.bing.com"),
                new UrlHash("QWERTY"),
                new UrlExpiration(DateTime.UtcNow.Date.AddDays(7)),
                new UrlVisits(11116),
                new UserId(2),
                new UrlCreationTime(DateTime.UtcNow.Date - TimeSpan.FromDays(2))
            ),
            new Url(
                new OriginalUrl("https://www.yahoo.com"),
                new UrlHash("ZXCV8N"),
                new UrlExpiration(DateTime.UtcNow.Date.AddDays(7)),
                new UrlVisits(33),
                new UserId(1),
                new UrlCreationTime(DateTime.UtcNow.Date - TimeSpan.FromDays(3))
            ),
            new Url(
                new OriginalUrl("https://www.duckduckgo.com"),
                new UrlHash("PK66YT"),
                new UrlExpiration(DateTime.UtcNow.Date.AddDays(7)),
                new UrlVisits(99),
                new UserId(2),
                new UrlCreationTime(DateTime.UtcNow.Date - TimeSpan.FromDays(2))
            ),
            new Url(
                new OriginalUrl("https://www.youtube.com"),
                new UrlHash("994RTY"),
                new UrlExpiration(DateTime.UtcNow.Date.AddDays(7)),
                new UrlVisits(600),
                new UserId(1),
                new UrlCreationTime(DateTime.UtcNow.Date - TimeSpan.FromDays(5))
            ),
            new Url(
                new OriginalUrl("https://www.twitch.tv"),
                new UrlHash("Z22V8N"),
                new UrlExpiration(DateTime.UtcNow.Date.AddDays(7)),
                new UrlVisits(7),
                new UserId(2),
                new UrlCreationTime(DateTime.UtcNow.Date - TimeSpan.FromDays(6))
            ),
        ];

        builder.HasData(urls);
    }
}