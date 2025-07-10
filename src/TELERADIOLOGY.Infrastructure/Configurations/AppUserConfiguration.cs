using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Infrastructure.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("Users");
        builder.Property(p => p.FirstName).HasColumnType("varchar(50)");
        builder.Property(p => p.LastName).HasColumnType("varchar(50)");
        builder.HasIndex(u => new { u.IsDeleted, u.LoginId, u.FirstName });
        builder.HasIndex(u => u.IdentityNumber).IsUnique();
    }
}
