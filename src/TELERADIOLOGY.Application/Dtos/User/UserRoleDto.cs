namespace TELERADIOLOGY.Application.Dtos.User;

public class UserRoleDto
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = default!;
}