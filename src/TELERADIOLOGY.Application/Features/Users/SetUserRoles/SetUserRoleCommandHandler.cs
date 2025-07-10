using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Roles.UserRoles.Commands
{
    internal sealed class SetUserRoleCommandHandler : IRequestHandler<SetUserRoleCommand, Unit>
    {
        private readonly IUserRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SetUserRoleCommandHandler(IUserRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SetUserRoleCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı zaten bu role sahip mi kontrolü yapalım
            var isRoleAssigned = await _roleRepository.AnyAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId, cancellationToken);

            if (isRoleAssigned)
            {
                throw new ArgumentException("Bu kullanıcı zaten bu role sahip.");
            }

            // Yeni UserRole nesnesi oluşturuluyor
            AppUserRole userRole = new AppUserRole
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            };

            // Role veritabanına ekleniyor
            await _roleRepository.AddAsync(userRole, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
