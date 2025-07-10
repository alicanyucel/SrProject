using MediatR;
using Microsoft.AspNetCore.Identity;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.AccountNotification.ResetPassword;

public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    private readonly UserManager<AppUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result<string>.Failure("Kullanıcı bulunamadı.");

        var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!resetResult.Succeeded)
        {
            var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
            return Result<string>.Failure($"Şifre sıfırlama başarısız: {errors}");
        }

        return Result<string>.Succeed("Şifreniz başarıyla sıfırlandı.");
    }
}
