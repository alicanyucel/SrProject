using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.DeleteMember;


public sealed record DeleteMemberByIdCommand(Guid Id) : IRequest<Result<string>>;