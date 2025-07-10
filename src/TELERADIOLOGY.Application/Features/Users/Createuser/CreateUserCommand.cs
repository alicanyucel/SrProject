using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Users.Createuser;
//
public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    string Repassword,
    string IdentityNumber,
    Guid RoleId,
    bool IsActive
) : IRequest<Result<string>>;
    