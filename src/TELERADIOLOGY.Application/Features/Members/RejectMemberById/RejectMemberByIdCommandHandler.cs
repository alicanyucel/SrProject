using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.RejectMemberById;

internal sealed class AddInfoMemberByIdCommandHandler(
    IMemberRepository memberRepository,
    ICacheService cacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<RejectMemberByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RejectMemberByIdCommand request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByExpressionWithTrackingAsync(x => x.Id == request.Id, cancellationToken);
        if (member is null)
        {
            return Result<string>.Failure(404, "Başvuru bulunamadı");
        }
        member.ApplicationStatus = Domain.Enums.ApplicationStatus.Rejected;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        cacheService.Remove("members");
        return "Üyelik başvurusu reddedildi";
    }
}