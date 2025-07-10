using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Hospitals.DeleteHospital;

public sealed record DeleteHospitalByIdCommand(
    Guid Id) : IRequest<Result<string>>;
