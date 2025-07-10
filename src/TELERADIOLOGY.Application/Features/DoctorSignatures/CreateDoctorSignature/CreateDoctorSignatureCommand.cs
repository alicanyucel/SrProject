using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.CreateDoctorSignature;

public record CreateDoctorSignatureCommand(
    string Degree,
    string DegreeNo,
    string DiplomaNo,
    string RegisterNo,
    string DisplayName,
    byte[] Signature
   ) : IRequest<Result<string>>;
