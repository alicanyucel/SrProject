using FluentValidation;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.CreateDoctorSignature;

public class CreateDoctorSignatureValidator : AbstractValidator<CreateDoctorSignatureCommand>
{
    public CreateDoctorSignatureValidator()
    {
        RuleFor(x => x.Degree)
            .NotEmpty().WithMessage("Ünvan (Degree) alanı boş olamaz.")
            .MaximumLength(100).WithMessage("Ünvan en fazla 100 karakter olabilir.");

        RuleFor(x => x.DegreeNo)
            .NotEmpty().WithMessage("Diploma no alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Diploma numarası en fazla 50 karakter olabilir.");

        RuleFor(x => x.DiplomaNo)
            .NotEmpty().WithMessage("Diploma no alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Diploma numarası en fazla 50 karakter olabilir.");

        RuleFor(x => x.RegisterNo)
            .NotEmpty().WithMessage("Sicil numarası (RegisterNo) boş olamaz.")
            .MaximumLength(50).WithMessage("Sicil numarası en fazla 50 karakter olabilir.");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Doktor adı (DisplayName) boş olamaz.")
            .MaximumLength(200).WithMessage("Doktor adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Signature)
            .NotNull().WithMessage("İmza (Signature) boş olamaz.")
            .Must(signature => signature.Length > 0).WithMessage("İmza verisi boş olamaz.");
    }
}
