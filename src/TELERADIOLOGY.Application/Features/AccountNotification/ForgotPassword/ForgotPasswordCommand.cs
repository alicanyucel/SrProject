using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.AccountNotification.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : IRequest<Result<string>>;
