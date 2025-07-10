using FluentValidation;
using TELERADIOLOGY.Application.Features.Users.Createuser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon numarası boş olamaz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

        RuleFor(x => x.Repassword)
            .Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor.");

        RuleFor(x => x.IdentityNumber)
            .NotEmpty().WithMessage("TC Kimlik No boş olamaz.")
            .Length(11).WithMessage("TC Kimlik No 11 haneli olmalıdır.")
            .Matches("^[0-9]{11}$").WithMessage("TC Kimlik No yalnızca sayılardan oluşmalıdır.");
    }
}
