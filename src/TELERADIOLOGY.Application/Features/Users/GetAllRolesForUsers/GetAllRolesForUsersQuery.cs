using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Users.GetAllRolesForUsers;

public record GetAllUserRolesQuery() : IRequest<List<AppUserRole>>;