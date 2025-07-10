using FluentValidation;
namespace TELERADIOLOGY.Application.Features.Companies.CreateCompany;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.CompanySmallTitle)
            .NotEmpty().WithMessage("Şirket kısa başlığı boş olamaz.")
            .MaximumLength(100).WithMessage("Şirket kısa başlığı en fazla 100 karakter olabilir.");

        RuleFor(x => x.CompanyTitle)
            .NotEmpty().WithMessage("Şirket başlığı boş olamaz.")
            .MaximumLength(250).WithMessage("Şirket başlığı en fazla 250 karakter olabilir.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon numarası boş olamaz.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Adres boş olamaz.");

        RuleFor(x => x.TaxNo)
            .NotEmpty().WithMessage("Vergi numarası boş olamaz.")
            .Length(10).WithMessage("Vergi numarası 10 haneli olmalıdır."); //türkkşyede 10 haneli

        RuleFor(x => x.WebSite)
            .NotEmpty().WithMessage("Web sitesi boş olamaz.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Şehir boş olamaz.");

        RuleFor(x => x.District)
            .NotEmpty().WithMessage("İlçe boş olamaz.");
    }
}
