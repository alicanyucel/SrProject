namespace TELERADIOLOGY.Application.Features.Roles.UserRoles.Dtos;
public record UserRoleDto
{
    public Guid UserId { get; init; }  
    public Guid RoleId { get; init; }    
}
