using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Infrastructure.Configurations;

internal sealed class PartitionConfiguration : IEntityTypeConfiguration<Partition>
{
    public void Configure(EntityTypeBuilder<Partition> builder)
    {
        builder.ToTable("Partitions");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.PartitionName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Modality)
            .HasMaxLength(50);

        builder.Property(p => p.ReferenceKey)
            .HasMaxLength(50);

        builder.Property(p => p.PartitionCode)
            .HasMaxLength(50);

        builder.Property(p => p.CompanyCode)
            .HasMaxLength(50);

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne<Company>()
            .WithMany(c => c.Partitions)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Hospital>()
            .WithMany(h => h.Partitions)
            .HasForeignKey(p => p.HospitalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
