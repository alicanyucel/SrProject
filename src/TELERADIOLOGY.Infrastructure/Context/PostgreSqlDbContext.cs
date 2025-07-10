using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Infrastructure.Repositories;

namespace TELERADIOLOGY.Infrastructure.Context;

public class PostgreSqlDbContext : IdentityDbContext<
    AppUser,
    AppRole,
    Guid,
    IdentityUserClaim<Guid>,
    AppUserRole,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>,
    IUnitOfWork
{
    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : base(options) { }
    public DbSet<DoctorSignature> DoctorSignatures { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<Report> Reports { get; set; }
    public DbSet<Partition> Partitions { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    public DbSet<UserHospitalPartition> UserHospitalPartitions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(PostgreSqlDbContext).Assembly);
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserRole<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Entity<AppUser>()
            .Ignore(u => u.Id);
        builder.Entity<AppUser>()
            .HasKey(u => u.Id);
        builder.Entity<AppUser>()
            .HasOne(u => u.Login)
            .WithOne(l => l.AppUser)
            .HasForeignKey<AppUser>(u => u.LoginId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Login>()
            .HasOne(l => l.AppUser)
            .WithOne(u => u.Login)
            .HasForeignKey<AppUser>(u => u.LoginId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AppUserRole>(b =>
        {
            b.HasKey(ur => new { ur.UserId, ur.RoleId });

            b.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<AppRole>(b =>
        {
            b.Property(r => r.Description)
                .HasMaxLength(256)
                .IsRequired(false);

            b.Property(r => r.CreatedAt)
                .IsRequired();
        });

        builder.Entity<CompanyUser>(b =>
        {
            b.HasKey(cu => new { cu.CompanyId, cu.UserId });

            b.HasOne(cu => cu.User)
                .WithMany(u => u.CompanyUsers)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(cu => cu.Company)
                .WithMany(c => c.CompanyUsers)
                .HasForeignKey(cu => cu.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<AppUser>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Company>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Hospital>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Member>().HasQueryFilter(x => !x.IsDeleted);
    }

    public override int SaveChanges()
    {
        NormalizeDateTimes();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        NormalizeDateTimes();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void NormalizeDateTimes()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.ClrType == typeof(DateTime))
                    {
                        var dt = (DateTime)property.CurrentValue!;
                        if (dt.Kind == DateTimeKind.Local)
                            property.CurrentValue = dt.ToUniversalTime();
                    }

                    if (property.Metadata.ClrType == typeof(DateTime?))
                    {
                        var dt = (DateTime?)property.CurrentValue;
                        if (dt.HasValue && dt.Value.Kind == DateTimeKind.Local)
                            property.CurrentValue = dt.Value.ToUniversalTime();
                    }
                }
            }
        }
    }
}
