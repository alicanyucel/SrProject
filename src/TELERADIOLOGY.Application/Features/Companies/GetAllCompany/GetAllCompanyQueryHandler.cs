using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Companies.GetAllCompany;

internal sealed class GetAllCompanyQueryHandler(
    ICompanyRepository companyRepository,
    ICacheService cacheService) : IRequestHandler<GetAllCompanyQuery, Result<List<Company>>>
{
    public async Task<Result<List<Company>>> Handle(GetAllCompanyQuery request, CancellationToken cancellationToken)
    {

        List<Company>? companies;

        companies = cacheService.Get<List<Company>>("companies");

        if (companies is null)
        {
            companies =
                await companyRepository
                .GetAll()
                .Include(cu => cu.CompanyUsers)
                .OrderBy(p => p.CompanyTitle)
                .ToListAsync(cancellationToken);

            cacheService.Set<List<Company>>("companies", companies);
        }

        return companies;
    }
}
