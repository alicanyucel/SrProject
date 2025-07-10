using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<AppUser>;
