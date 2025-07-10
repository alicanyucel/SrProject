using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Features.Members.GetByIdMember;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;


internal sealed class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, Member>
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByIdQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Member> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        return await _memberRepository.GetAll().SingleAsync(r => r.Id == request.Id, cancellationToken);
    }
}