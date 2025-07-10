using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.GetAllDoctorSignature;
public sealed record GetAllDoctorSignatureQuery() : IRequest<Result<List<DoctorSignature>>>;
