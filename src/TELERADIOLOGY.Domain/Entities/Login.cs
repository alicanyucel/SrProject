using System.ComponentModel.DataAnnotations;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class Login
{
    [Key]
    public Guid LoginId { get; set; }  
    public Guid RoleId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool Isactive { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? LastLogin { get; set; }
    public string UserCode { get; set; } 
    public AppUser AppUser { get; set; } 
}
