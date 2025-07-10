using MediatR;
using TS.Result;

public sealed record AddInfoMemberByIdCommand(
    Guid Id,string Description) : IRequest<Result<string>>;
