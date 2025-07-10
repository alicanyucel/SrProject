using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.UpdatePermission;

public sealed record UpdatePermissionCommand(int Id, string EndPoint, string Method, string Description)
    : IRequest<Result<string>>;
