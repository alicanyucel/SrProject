using FluentValidation;
using TELERADIOLOGY.Application.Extensions;

namespace TELERADIOLOGY.Application.Features.Members.CreateMember;
public class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir.");

        RuleFor(x => x.IdentityNumber)
            .NotEmpty().WithMessage("TC Kimlik No boş olamaz.")
            .Length(11).WithMessage("TC Kimlik No 11 haneli olmalıdır.")
            .Matches(@"^\d{11}$").WithMessage("TC Kimlik No yalnızca rakamlardan oluşmalıdır.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefon numarası girilmelidir.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Telefon numarası geçerli değil.");
       
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçersiz email adresi.");

        RuleFor(x => x.AreaOfInterest)
            .NotNull().WithMessage("İlgi alanı zorunludur.")
            .IsValidSmartEnum().WithMessage("Geçersiz ilgi alanı.");
    }
}
