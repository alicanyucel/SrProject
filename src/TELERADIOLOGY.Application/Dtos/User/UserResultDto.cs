namespace TELERADIOLOGY.Application.Dtos.User;

public class UserResultDto
{ 
    public Guid Id { get;  set; }
    public string IdentityNumber { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public Guid LoginId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string UserCode { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<UserRoleDto>? UserRoles { get; set; }        
}