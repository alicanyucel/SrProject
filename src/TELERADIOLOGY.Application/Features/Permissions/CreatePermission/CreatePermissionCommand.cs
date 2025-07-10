using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.CreatePermission;

public sealed record CreatePermissionCommand(string EndPoint,string Method,string Description) : IRequest<Result<string>>;
