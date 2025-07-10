using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Extensions;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.UpdatePermission;

public sealed class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Result<string>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePermissionCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
    {
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {

        var permission = await _permissionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (permission is null)
            return Result<string>.Failure("İzin bulunamadı.");

        permission.EndPoint = request.EndPoint;
        permission.Method = request.Method.ToUpperInvariant();
        permission.Description = request.Description;

        _permissionRepository.Update(permission);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("İzin güncellendi.");
    }
}
