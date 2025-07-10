using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Dtos.Hospital;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Hospitals.HospitalFilter;

internal sealed class GetHospitalsByFilterQueryHandler(
    IHospitalRepository hospitalRepository,
    IMapper mapper) : IRequestHandler<GetHospitalsByFilterQuery, Result<List<HospitalResultDto>>>
{
    //
    public async Task<Result<List<HospitalResultDto>>> Handle(GetHospitalsByFilterQuery request, CancellationToken cancellationToken)
    {
        var query = hospitalRepository.GetAll();
        if (!string.IsNullOrWhiteSpace(request.Filter))
        {
            var filter = $"%{request.Filter}%";
            query = query.Where(c =>
                EF.Functions.ILike(c.ShortName, filter) ||
                EF.Functions.ILike(c.FullTitle, filter) ||
                EF.Functions.ILike(c.AuthorizedPerson, filter) ||
                EF.Functions.ILike(c.City, filter) ||
                EF.Functions.ILike(c.District, filter) ||
                EF.Functions.ILike(c.Email, filter) ||
                EF.Functions.ILike(c.Address, filter) ||
                EF.Functions.ILike(c.TaxNumber, filter) ||
                EF.Functions.ILike(c.TaxOffice, filter) ||
                EF.Functions.ILike(c.Website, filter)
            );
        }
        if (request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive.Value);
        }
        var hospitals = await query
            .ProjectTo<HospitalResultDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return Result<List<HospitalResultDto>>.Succeed(hospitals);
    }
}
