using MediatR;
using TELERADIOLOGY.Application.Dtos.Hospital;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Hospitals.HospitalFilter;

public sealed record GetHospitalsByFilterQuery(
    string Filter,
    bool? IsActive) : IRequest<Result<List<HospitalResultDto>>>;
