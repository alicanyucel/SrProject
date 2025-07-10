using MediatR;
using TELERADIOLOGY.Application.Dtos.Permission;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.GetAllPermission;

public sealed class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, Result<List<PermissionDto>>>
{
    private readonly IPermissionRepository _permissionRepository;

    public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Result<List<PermissionDto>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await Task.Run(() =>
            _permissionRepository
                .GetAll()
                .Where(p => !p.IsDeleted) 
                .ToList(), cancellationToken);

        var dtoList = permissions.Select(p => new PermissionDto
        {
            Id = p.Id,
            EndPoint = p.EndPoint,
            Method = p.Method,
            Description = p.Description
        }).ToList();
        return Result<List<PermissionDto>>.Succeed(dtoList);
    }
}
