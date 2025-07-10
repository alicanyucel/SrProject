using MediatR;
using TELERADIOLOGY.Domain.Enums;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.CreateMember;

public sealed record CreateMemberCommand(
    string FirstName,
    string LastName,
    string IdentityNumber,
    string Phone,
    string Email,
    AreaOfInterest AreaOfInterest) : IRequest<Result<string>>;
