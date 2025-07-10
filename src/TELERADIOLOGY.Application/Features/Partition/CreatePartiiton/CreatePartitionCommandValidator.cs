using FluentValidation;
using TELERADIOLOGY.Application.Features.Partitions.CreatePartition;

public class CreatePartitionCommandValidator : AbstractValidator<CreatePartitionCommand>
{
    public CreatePartitionCommandValidator()
    {
        RuleFor(x => x.PartitionName)
            .NotEmpty().WithMessage("Partition adı zorunludur.")
            .MaximumLength(100).WithMessage("Partition adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.HospitalId)
            .NotEmpty().WithMessage("HospitalId alanı zorunludur.");

        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("CompanyId alanı zorunludur.");

        RuleFor(x => x.Modality)
            .NotEmpty().WithMessage("Modality (modalite) alanı zorunludur.")
            .MaximumLength(50).WithMessage("Modality en fazla 50 karakter olabilir.");

        RuleFor(x => x.PartitionCode)
            .NotEmpty().WithMessage("Partition kodu zorunludur.")
            .MaximumLength(50).WithMessage("Partition kodu en fazla 50 karakter olabilir.");

        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("Company kodu zorunludur.")
            .MaximumLength(50).WithMessage("Company kodu en fazla 50 karakter olabilir.");

        RuleFor(x => x.ReferenceKey)
            .NotEmpty().WithMessage("Referans anahtarı (ReferenceKey) zorunludur.")
            .MaximumLength(50).WithMessage("Referans anahtarı en fazla 50 karakter olabilir.");
    }
}
