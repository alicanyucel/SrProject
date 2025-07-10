using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.CreatePermission;

public sealed class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Result<string>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePermissionCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
    {
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        bool exists = await _permissionRepository.AnyAsync(p =>
            p.EndPoint == request.EndPoint && p.Method == request.Method, cancellationToken);

        if (exists)
        {
            return Result<string>.Failure("izin zaten eklenmiş.");
        }
        var permission = new Permission
        {
            EndPoint = request.EndPoint,
            Method = request.Method.ToUpperInvariant(),
            Description = request.Description
        };
        await _permissionRepository.AddAsync(permission, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("İzinler oluşturuldu.");
    }
}
