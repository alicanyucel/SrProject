using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Users.UpdateUser;

public sealed record UpdateUserByIdCommand(
 Guid Id,
 Guid LoginId,
 string FirstName,
 string LastName, 
 string PhoneNumber,
 string Email,
 string? Password,
 string? Repassword,
 Guid RoleId,
 bool IsActive,
 string IdentityNumber,
 DateTime CreatedDate
) : IRequest<Result<string>>;
