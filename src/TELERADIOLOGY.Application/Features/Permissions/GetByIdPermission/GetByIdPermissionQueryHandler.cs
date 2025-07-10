using MediatR;
using TELERADIOLOGY.Application.Dtos.Permission;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.GetByIdPermission;

public sealed class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, Result<PermissionDto>>
{
    private readonly IPermissionRepository _permissionRepository;

    public GetPermissionByIdQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Result<PermissionDto>> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByExpressionAsync(
            p => p.Id == request.Id, cancellationToken);

        if (permission is null)
            return Result<PermissionDto>.Failure("İzin bulunamadı.");

        var dto = new PermissionDto
        {
            Id = permission.Id,
            EndPoint = permission.EndPoint,
            Method = permission.Method,
            Description = permission.Description
        };
        return Result<PermissionDto>.Succeed(dto);
    }
}
