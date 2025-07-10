using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.CreateMember;

internal sealed class CreateMemberCommandHandler(
    IMemberRepository memberRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<CreateMemberCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {

        var existingMember = await memberRepository.GetByExpressionAsync(x => x.Email == request.Email, cancellationToken);

        if (existingMember is not null)
        {
            return Result<string>.Failure("Bu email'e ait kullanıcı daha önce başvurulmuştur");
        }

        Member member = mapper.Map<Member>(request);
        member.ApplicationStatus = Domain.Enums.ApplicationStatus.Unapproved;

        memberRepository.Add(member);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        cacheService.Remove("members");
        return "Üye başvurusu yapıldı";
    }
}
