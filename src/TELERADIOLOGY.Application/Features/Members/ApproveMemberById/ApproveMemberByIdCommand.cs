using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.ApproveMemberById;
public sealed record ApproveMemberByIdCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    string Repassword,
    string IdentityNumber,
    Guid RoleId) : IRequest<Result<string>>;
