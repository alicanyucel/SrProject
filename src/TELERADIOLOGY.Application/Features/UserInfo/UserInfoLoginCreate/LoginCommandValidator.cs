using FluentValidation;

namespace TELERADIOLOGY.Application.Features.UserInfo.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage("Rol ID değeri 0'dan büyük olmalıdır.");

        RuleFor(x => x.username)
            .NotEmpty()
            .WithMessage("Kullanıcı adı boş olamaz.")
            .MaximumLength(50)
            .WithMessage("Kullanıcı adı en fazla 50 karakter olabilir.");

        RuleFor(x => x.password)
            .NotEmpty()
            .WithMessage("Şifre boş olamaz.")
            .MinimumLength(6)
            .WithMessage("Şifre en az 6 karakter olmalıdır.")
            .MaximumLength(100)
            .WithMessage("Şifre en fazla 100 karakter olabilir.");

        RuleFor(x => x.created_date)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Oluşturulma tarihi bugünden ileri bir tarih olamaz.");

        RuleFor(x => x.last_login)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Son giriş tarihi bugünden ileri bir tarih olamaz.");

        RuleFor(x => x.user_code)
            .NotEmpty()
            .WithMessage("Kullanıcı kodu boş olamaz.")
            .MaximumLength(20)
            .WithMessage("Kullanıcı kodu en fazla 20 karakter olabilir.");
    }
}
