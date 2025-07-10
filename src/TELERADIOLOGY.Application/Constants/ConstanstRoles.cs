using TELERADIOLOGY.Domain.Entities;

public static class ConstantsRoles
{
    public static List<AppRole> GetRoles()
    {
        return new List<AppRole>
        {
            new AppRole
            {
                Id= Guid.Parse("5ed62427-ec28-46ce-be0e-376005fae043"),
                Name = "Admin",
                RoleName = "Admin",
                NormalizedName = "ADMIN",
                Description = "Full access to all system features, user management, settings, and reporting.",
                CreatedAt = DateTime.UtcNow
            },
            new AppRole
            {
                Id = Guid.Parse("46fecbbe-79b3-4a0c-85d2-a7846d901272"),
                Name = "System",
                RoleName="System",
                NormalizedName = "SYSTEM",
                Description = "System-level operations and configurations.",
                CreatedAt = DateTime.UtcNow
            },
            new AppRole
            {
                Id= Guid.Parse("e5b650ef-c509-42bd-8e9f-f784cbb338ed"),
                Name = "Reporter",
                RoleName="Reporter",
                NormalizedName = "REPORTER",
                Description = "Access to reporting tools and view-only permissions.",
                CreatedAt = DateTime.UtcNow
            },
            new AppRole
            {
                Id = Guid.Parse("47d90a18-da9e-4fe7-9199-7b73697f2001"),
                Name = "Hospital",
                RoleName="Hospital",
                NormalizedName = "HOSPITAL",
                Description = "Manages hospital-related data and users.",
                CreatedAt = DateTime.UtcNow
            },
            new AppRole
            {
                Id = Guid.Parse("ba9c54b2-d78b-44e2-aa7e-33c85aab0497"),
                Name = "Doctor",
                RoleName = "Doctor",
                NormalizedName = "DOCTOR",
                Description = "Access to patient records and medical tools.",
                CreatedAt = DateTime.UtcNow
            },
            new AppRole
            {
                // giy sync
                Id = Guid.Parse("58d79190-fa0a-4a75-befb-a11b270b1b0e"),
                Name = "Company",
                RoleName = "Company",
                NormalizedName = "COMPANY",
                Description = "Company-specific operations and reporting.",
                CreatedAt = DateTime.UtcNow
            }
        };
    }
}
