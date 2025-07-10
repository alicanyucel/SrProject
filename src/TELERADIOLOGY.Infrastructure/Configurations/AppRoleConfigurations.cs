using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Infrastructure.Configurations;

internal sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable("Roles");

        builder.Property(r => r.Description)
               .HasMaxLength(500)
               .HasColumnName("Description");

        builder.Property(r => r.CreatedAt)
               .HasColumnName("CreatedAt")
               .HasDefaultValueSql("NOW()"); 
    }
}
