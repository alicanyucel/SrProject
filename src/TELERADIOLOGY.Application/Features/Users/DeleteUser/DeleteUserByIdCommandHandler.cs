using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;
using GenericRepository;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Application.Services;

namespace TELERADIOLOGY.Application.Features.Users.DeleteUser;

internal sealed class DeleteUserByIdCommandHandler(
    IUserRepository userRepository,
    ICacheService cacheService,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteUserByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        AppUser user = await userRepository.GetByExpressionWithTrackingAsync(
            p => p.Id == request.Id && !p.IsDeleted,
            cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }
        user.IsDeleted = true;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        cacheService.Remove("users");

        return "Kullanıcı başarıyla silindi.";
    }
}
