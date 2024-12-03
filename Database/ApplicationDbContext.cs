using Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, Role, UserId>(options)
{
    public virtual DbSet<Url> Urls { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Url>(entity =>
        {
            entity.HasKey(e => e.Hash);

            entity.Property(e => e.Hash)
                .HasConversion(v => v.Value, v => new UrlHash(v))
                .IsRequired();

            entity.Property(e => e.OriginalUrl)
                .HasConversion(v => v.Value, v => new OriginalUrl(v))
                .IsRequired();

            entity.Property(e => e.Expiration)
                .HasConversion(v => v.Value, v => new UrlExpiration(v))
                .IsRequired();

            entity.Property(e => e.Visits)
                .HasConversion(v => v.Value, v => new UrlVisits(v))
                .IsRequired();

            entity.Property(e => e.CreatedBy)
                .HasConversion(v => v.Value, v => new UserId(v))
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasConversion(v => v.Value, v => new UrlCreationTime(v))
                .IsRequired();
        });

        builder.Entity<User>(entity =>
        {
            entity.HasMany<Url>()
                .WithOne()
                .HasForeignKey(e => e.CreatedBy);

            entity.Property(e => e.Id)
                .HasConversion(
                    v => v.Value,
                    v => new UserId(v));
        });

        builder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id)
                .HasConversion(
                    v => v.Value,
                    v => new UserId(v));
        });

    }
}
