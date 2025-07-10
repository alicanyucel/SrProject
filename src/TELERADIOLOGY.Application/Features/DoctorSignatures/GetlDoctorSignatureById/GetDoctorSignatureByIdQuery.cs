using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.GetAllDoctorSignatureById;


public sealed record GetDoctorSignatureByIdQuery(Guid Id) : IRequest<Result<DoctorSignature>>;
