using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Application.Features.Hospitals.GetHospitalById;
internal sealed class GetHospitalByIdQueryHandler : IRequestHandler<GetHospitalByIdQuery, Hospital>
{
    private readonly IHospitalRepository _hospitalRepository;

    public GetHospitalByIdQueryHandler(IHospitalRepository hospitalRepository)
    {
        _hospitalRepository = hospitalRepository;
    }

    public async Task<Hospital> Handle(GetHospitalByIdQuery request, CancellationToken cancellationToken)
    {
        return await _hospitalRepository.GetAll().SingleAsync(h => h.Id == request.Id, cancellationToken);
    }
}