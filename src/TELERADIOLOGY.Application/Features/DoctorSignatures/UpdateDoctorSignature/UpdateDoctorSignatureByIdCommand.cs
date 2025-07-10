using TELERADIOLOGY.Application.Features.DoctorSignatures.CreateDoctorSignature;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.UpdateDoctorSignature;

public sealed record UpdateDoctorSignatureByIdCommand : CreateDoctorSignatureCommand
{
    public UpdateDoctorSignatureByIdCommand(string Degree, string DegreeNo, string DiplomaNo, string RegisterNo, string DisplayName, byte[] Signature) : base(Degree, DegreeNo, DiplomaNo, RegisterNo, DisplayName, Signature)
    {
    }

    public Guid Id { get; set; }
}
