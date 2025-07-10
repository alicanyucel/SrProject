using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Enums;

namespace TELERADIOLOGY.Infrastructure.Configurations
{
    internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(p => p.CompanyType).HasConversion(v => v.Value, v => CompanyType.FromValue(v)).HasColumnName("CompanyType");
        }
    }
}
