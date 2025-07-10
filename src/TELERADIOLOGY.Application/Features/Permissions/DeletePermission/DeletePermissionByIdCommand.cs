using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.DeletePermission;

public sealed record DeletePermissionByIdCommand(int PermissionId) : IRequest<Result<string>>;
