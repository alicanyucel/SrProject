using MediatR;
using TS.Result;

public record SendEmailCommand(string To, string Subject, string Message) : IRequest<Result<string>>;