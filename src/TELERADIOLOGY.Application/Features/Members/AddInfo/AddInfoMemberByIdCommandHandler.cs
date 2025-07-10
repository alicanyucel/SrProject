using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Enums;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

internal sealed class AddInfoMemberByIdCommandHandler(
    IMemberRepository memberRepository,
    ICacheService cacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<AddInfoMemberByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddInfoMemberByIdCommand request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByExpressionWithTrackingAsync(x => x.Id == request.Id, cancellationToken);
        if (member is null)
        {
            return Result<string>.Failure(404, "Başvuru bulunamadı");
        }

        member.ApplicationStatus = ApplicationStatus.AdditionalInformationRequested;
        member.AddInfoMessage = request.Description; 

        await unitOfWork.SaveChangesAsync(cancellationToken);
        cacheService.Remove("members");
        return "Üyelik başvurusunda ek bilgi istendi.";
    }
}