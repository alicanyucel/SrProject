using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Hospitals.GetAllHospital;
public sealed record GetAllHospitalQuery() : IRequest<Result<List<Hospital>>>;
