using Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, Role, UserId>(options)
{
    public DbSet<Url> Urls => Set<Url>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Url>(entity =>
        {
            entity.HasKey(e => e.Hash);
            entity.Property(e => e.OriginalUrl).IsRequired();
            entity.Property(e => e.Hash).IsRequired();
            entity.Property(e => e.Info).IsRequired();

            entity.ComplexProperty(e => e.Info, info =>
            {
                info.Property(e => e.Expiration).IsRequired();
                info.Property(e => e.Visits).IsRequired();
                info.Property(e => e.CreatedBy).IsRequired();
                info.Property(e => e.CreatedAt).IsRequired();
            });
        });
    }
}