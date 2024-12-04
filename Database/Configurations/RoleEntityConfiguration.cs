using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

internal class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {

        builder.Property(e => e.Id)
            .HasConversion(
                v => v.Value,
                v => new UserId(v));

        builder.HasData(
            new Role { Id = new UserId(1), Name = "Admin", NormalizedName = "ADMIN" },
            new Role { Id = new UserId(2), Name = "User", NormalizedName = "USER" }
        );

    }
}

internal class UserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityUserRole<UserId>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<UserId>> builder)
    {
        builder.HasData(
            new IdentityUserRole<UserId> { UserId = new UserId(1), RoleId = new UserId(1) },
            new IdentityUserRole<UserId> { UserId = new UserId(2), RoleId = new UserId(2) }
        );
    }
}