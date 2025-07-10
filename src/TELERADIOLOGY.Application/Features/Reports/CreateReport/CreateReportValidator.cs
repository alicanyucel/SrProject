using FluentValidation;

namespace TELERADIOLOGY.Application.Features.Reports.CreateReport
{
    public class CreateReportValidator : AbstractValidator<CreateReportCommand>
    {
        public CreateReportValidator()
        {
            RuleFor(x => x.ReportName)
                .NotEmpty().WithMessage("Name alanı boş olamaz.")
                .MaximumLength(100).WithMessage("Name en fazla 100 karakter olabilir.");

            RuleFor(x => x.ModalityType)
                .NotEmpty().WithMessage("ModalityType zorunludur.")
                .MaximumLength(50).WithMessage("ModalityType en fazla 50 karakter olabilir.");

            RuleFor(x => x.TemplateId)
                .NotEmpty().WithMessage("TemplateId boş olamaz.")
                .Must(id => id != Guid.Empty).WithMessage("Geçersiz TemplateId.");
        }
    }
}
