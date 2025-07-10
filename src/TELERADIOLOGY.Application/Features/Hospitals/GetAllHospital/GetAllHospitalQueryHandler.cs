using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Hospitals.GetAllHospital;

internal sealed class GetAllHospitalQueryHandler(
   IHospitalRepository hospitalRepository) : IRequestHandler<GetAllHospitalQuery, Result<List<Hospital>>>
{
    public async Task<Result<List<Hospital>>> Handle(GetAllHospitalQuery request, CancellationToken cancellationToken)
    {
        var hospitals = await Task.Run(() => hospitalRepository.GetAll().OrderBy(p => p.FullTitle).ToList());
        return Result<List<Hospital>>.Succeed(hospitals);
    }
}
