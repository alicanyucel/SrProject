using Microsoft.AspNetCore.Identity;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class AppRole : IdentityRole<Guid> 
{
    // sorun çözüldü
    public ICollection<AppUserRole> UserRoles { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
