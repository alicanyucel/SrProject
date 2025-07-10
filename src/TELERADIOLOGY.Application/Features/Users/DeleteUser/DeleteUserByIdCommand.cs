using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Users.DeleteUser;

public sealed record DeleteUserByIdCommand(Guid Id) : IRequest<Result<string>>;
