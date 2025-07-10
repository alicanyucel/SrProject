using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Sms;

public record SendSmsCommand(string PhoneNumber, string Message) : IRequest<Result<string>>;

