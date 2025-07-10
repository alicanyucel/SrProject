using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Extensions;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Permissions.DeletePermission
{
    public sealed class DeletePermissionByIdCommandHandler : IRequestHandler<DeletePermissionByIdCommand, Result<string>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePermissionByIdCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(DeletePermissionByIdCommand request, CancellationToken cancellationToken)
        {
            // Direkt int kullan
            var permission = await _permissionRepository.GetByIdAsync(request.PermissionId, cancellationToken);

            if (permission == null || permission.IsDeleted)
            {
                return Result<string>.Failure("İzin zaten silinmiş.");
            }

            permission.IsDeleted = true;
            _permissionRepository.Update(permission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("İzin soft delete silindi.");
        }
    }
}