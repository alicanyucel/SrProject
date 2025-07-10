using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.GetAllMember;

internal sealed class GetAllMemberQueryHandler(
    IMemberRepository memberRepository,
    ICacheService cacheService) : IRequestHandler<GetAllMemberQuery, Result<List<Member>>>
{
    public async Task<Result<List<Member>>> Handle(GetAllMemberQuery request, CancellationToken cancellationToken)
    {
        List<Member>? members;

        members = cacheService.Get<List<Member>>("members");

        if (members is null)
        {
            members =
                await memberRepository
                .GetAll()
                .OrderBy(m => m.FirstName)
                .ToListAsync(cancellationToken);

            cacheService.Set<List<Member>>("members", members);
        }
        return members;
    }
}