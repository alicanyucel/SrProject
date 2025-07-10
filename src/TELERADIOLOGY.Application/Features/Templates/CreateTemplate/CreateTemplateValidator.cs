using FluentValidation;

namespace TELERADIOLOGY.Application.Features.Templates.CreateTemplate
{
    public class CreateTemplateValidator : AbstractValidator<CreateTemplateCommand>
    {
        public CreateTemplateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name alanı boş olamaz.")
                .MaximumLength(100).WithMessage("Name en fazla 150 karakter olabilir.");

            RuleFor(x => x.RaporTipi)
                .NotEmpty().WithMessage("Rapor Tipi zorunludur.")
                .MaximumLength(50).WithMessage("Rapor Tipi en fazla 150 karakter olabilir.");

            RuleFor(x => x.ContextHtml)
                .NotEmpty().WithMessage("ContextHtml alanı boş olamaz.");
        }
    }
}
