using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.DeleteDoctorSignature;

public sealed record DeleteDoctorSignatureByIdCommand(Guid SignatureId) : IRequest<Result<string>>;