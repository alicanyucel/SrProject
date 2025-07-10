using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.AccountNotification.ResetPassword;

public sealed record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<Result<string>>;
