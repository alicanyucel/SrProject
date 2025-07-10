using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Hospitals.GetHospitalById;

public sealed record GetHospitalByIdQuery(Guid Id) : IRequest<Hospital>;
