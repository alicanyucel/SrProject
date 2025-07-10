using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class AppUser : IdentityUser<Guid>
{
    public Guid? LoginId { get; set; }
    [Required(ErrorMessage = "TC Kimlik No boş olamaz.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır.")]
    [RegularExpression("^[0-9]{11}$", ErrorMessage = "TC Kimlik No yalnızca rakamlardan oluşmalıdır.")]
    public string IdentityNumber { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; }
    public string UserCode { get; set; } = default!;
    public Login Login { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
    public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    
}
