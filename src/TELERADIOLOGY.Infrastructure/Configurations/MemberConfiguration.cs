using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Enums;

namespace TELERADIOLOGY.Infrastructure.Configurations;

internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{

    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(p => p.ApplicationStatus)
            .HasConversion(v => v.Value, v => ApplicationStatus.FromValue(v))
            .HasColumnName("ApplicationStatus");

        builder.Property(p => p.AreaOfInterest)
            .HasConversion(v => v.Value, v => AreaOfInterest.FromValue(v))
            .HasColumnName("AreaOfInterest");

    }
}

