using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUsersByCompanyId;

internal sealed class GetCompanyUsersByCompanyIdQueryHandler(
    ICompanyUserRepository companyUserRepository) : IRequestHandler<GetCompanyUsersByCompanyIdQuery, Result<List<GetCompanyUsersByCompanyIdQueryResponse>>>
{
    public async Task<Result<List<GetCompanyUsersByCompanyIdQueryResponse>>> Handle(GetCompanyUsersByCompanyIdQuery request, CancellationToken cancellationToken)
    {
        var companyUsers = await companyUserRepository
            .Where(x => x.CompanyId == request.CompanyId && x.IsDeleted == false)
            .Include(x => x.User)
            .ToListAsync(cancellationToken);

        if (companyUsers is null || companyUsers.Count == 0)
        {
            return Result<List<GetCompanyUsersByCompanyIdQueryResponse>>.Succeed(new());
        }

        var response = companyUsers
            .Where(x => x.User != null)
            .Select(x => new GetCompanyUsersByCompanyIdQueryResponse(
                x.UserId,
                x.User.FirstName,
                x.User.LastName,    
                x.User.Phone,
                x.User.Email
                ))
            .ToList();

        return response;
    }
}
