using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TELERADIOLOGY.Domain.Entities;

public class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUser>
{
    public void Configure(EntityTypeBuilder<CompanyUser> builder)
    {
        builder.HasKey(cu => new { cu.CompanyId, cu.UserId });

        builder
            .HasOne(cu => cu.Company)
            .WithMany(c => c.CompanyUsers)
            .HasForeignKey(cu => cu.CompanyId);

        builder
            .HasOne(cu => cu.User)
            .WithMany(u => u.CompanyUsers)
            .HasForeignKey(cu => cu.UserId);

        builder.Property(cu => cu.IsActive).IsRequired();
        builder.Property(cu => cu.StartDate).IsRequired();
        builder.Property(cu => cu.EndDate).IsRequired(false);
        builder.Property(cu => cu.CreatedAt).IsRequired();
        builder.Property(cu => cu.UpdatedAt).IsRequired(false);
    }
}
