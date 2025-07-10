using MediatR;

public sealed record SetUserRoleCommand(
        Guid UserId,
        Guid RoleId
    ) : IRequest<Unit>;