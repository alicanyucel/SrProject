using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Roles.RoleSync;

public sealed record RoleSyncCommand() : IRequest<Result<string>>;