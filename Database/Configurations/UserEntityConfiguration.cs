using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany<Url>()
            .WithOne()
            .HasForeignKey(e => e.CreatedBy);

        builder.Property(e => e.Id).ValueGeneratedOnAdd()
            .HasConversion(
                v => v.Value,
                v => new UserId(v));

        builder.Property(e => e.UserName)
            .IsRequired();

        builder.Property(e => e.Email)
            .IsRequired();

        builder.HasKey(e => e.Id);


        List<User> users =
        [
            new User
            {
                Id = new UserId(1),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = null,
                ConcurrencyStamp = "f9cc2eea-807c-4b16-9148-ea4d89271064",
                PasswordHash = "AQAAAAIAAYagAAAAEPhL1BeLwSPmG5wdbQANirheBdXIWcowX5AcraI2Ze9IfaH9dFseZvkSzfuzlwoTJg=="
            },
            new User
            {
                Id = new UserId(2),
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@email.com",
                NormalizedEmail = "USER@EMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = null,
                ConcurrencyStamp = "07e37c7d-4882-4d1a-a23c-d10f86c7a46e",
                PasswordHash = "AQAAAAIAAYagAAAAEFlXKDB1j50Xqk3HmERBI83lHYjdxI4ML8MsIG5mFDc0AWeGl4+7mvHQ2DIZTF4inA=="

            }
        ];
        builder.HasData(users);
    }
}