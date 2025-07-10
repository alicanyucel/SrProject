using MediatR;
using TELERADIOLOGY.Application.Dtos.Permission;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.GetByIdPermission;

public sealed record GetPermissionByIdQuery(int Id) : IRequest<Result<PermissionDto>>;
