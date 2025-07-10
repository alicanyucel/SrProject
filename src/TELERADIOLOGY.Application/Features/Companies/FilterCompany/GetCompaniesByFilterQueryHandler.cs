using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Dtos.Company;
using TELERADIOLOGY.Application.Features.Companies.FilterCompany;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

internal sealed class GetCompaniesByFilterQueryHandler(
    ICompanyRepository companyRepository,
    IMapper mapper)
    : IRequestHandler<GetCompaniesByFilterQuery, Result<List<CompanyResultDto>>>
{
    public async Task<Result<List<CompanyResultDto>>> Handle(GetCompaniesByFilterQuery request, CancellationToken cancellationToken)
    {
        var query = companyRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(request.Filter))
        {
            var filter = $"%{request.Filter}%";

            query = query.Where(c =>
                EF.Functions.ILike(c.City, filter) ||
                EF.Functions.ILike(c.CompanyTitle, filter) ||
                EF.Functions.ILike(c.PhoneNumber, filter) ||
                EF.Functions.ILike(c.Address, filter) ||
                EF.Functions.ILike(c.CompanySmallTitle, filter) ||
                EF.Functions.ILike(c.Representative, filter) ||
                EF.Functions.ILike(c.TaxNo, filter) ||
                EF.Functions.ILike(c.TaxOffice, filter));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(c => c.Status == request.Status.Value);
        }

        if (request.Type.HasValue)
        {
            query = query.Where(c => c.CompanyType == request.Type.Value);
        }

        var entities = await query.ToListAsync(cancellationToken);
        var dtos = entities.Select(c => mapper.Map<CompanyResultDto>(c)).ToList();

        return Result<List<CompanyResultDto>>.Succeed(dtos);
    }
}
