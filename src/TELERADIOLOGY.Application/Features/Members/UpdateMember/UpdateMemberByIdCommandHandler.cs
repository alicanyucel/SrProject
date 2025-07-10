using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.UpdateMember;

internal sealed class UpdateMemberByIdCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateMemberByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateMemberByIdCommand request, CancellationToken cancellationToken)
    {
        Member member = await memberRepository.GetByExpressionWithTrackingAsync(m => m.Id == request.Id, cancellationToken);
        if (member == null)
        {
            return Result<string>.Failure("üye yok");
        }
        mapper.Map(request, member);
        memberRepository.Update(member);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Üye güncellendi.";

    }
}