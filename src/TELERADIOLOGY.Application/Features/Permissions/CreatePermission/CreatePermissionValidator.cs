using FluentValidation;

namespace TELERADIOLOGY.Application.Features.Permissions.CreatePermission;
public class CreatePermissionValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionValidator()
    {
        RuleFor(x => x.EndPoint)
            .NotEmpty().WithMessage("EndPoint is required.")
            .MaximumLength(200).WithMessage("EndPoint must be at most 200 characters.");

        RuleFor(x => x.Method)
            .NotEmpty().WithMessage("Method is required.")
            .Must(BeAValidHttpMethod).WithMessage("Method must be a valid HTTP method (GET, POST, PUT, DELETE, etc).");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must be at most 500 characters.");
    }

    private bool BeAValidHttpMethod(string method)
    {
        var validMethods = new[] { "GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS", "HEAD" };
        return validMethods.Contains(method?.ToUpper());
    }
}
