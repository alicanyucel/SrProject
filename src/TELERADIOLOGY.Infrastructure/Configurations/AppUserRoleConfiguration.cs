using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Infrastructure.Configurations;

internal sealed class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.ToTable("UserRoles");
    }
}
