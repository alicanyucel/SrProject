using Microsoft.AspNetCore.Identity;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class AppUserRole : IdentityUserRole<Guid>
{
    public  AppUser User { get; set; } 
    public  AppRole Role { get; set; } 
}