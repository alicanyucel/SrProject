using MediatR;
using TELERADIOLOGY.Application.Dtos.Permission;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.GetAllPermission;

public sealed record GetAllPermissionsQuery() : IRequest<Result<List<PermissionDto>>>;
