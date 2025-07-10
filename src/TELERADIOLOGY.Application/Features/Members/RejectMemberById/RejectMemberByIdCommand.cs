using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.RejectMemberById;
public sealed record RejectMemberByIdCommand(
    Guid Id) : IRequest<Result<string>>;
