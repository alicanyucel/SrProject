using MediatR;
using Microsoft.AspNetCore.Identity;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Services.NetMail;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.AccountNotification.ForgotPassword;

public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailSender _emailSender;

    public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        Console.WriteLine($"Password Reset Token: {token}");
        var resetLink = $"https://yourfrontend.com/reset-password?email={request.Email}&token={Uri.EscapeDataString(token)}";

        await _emailSender.SendEmailAsync(user.Email, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için <a href=\"{resetLink}\">buraya tıklayın</a>.");

        return Result<string>.Succeed(token);
    }
}
