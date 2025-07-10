using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Roles.GetRoles;

public class GetRolesQuery : IRequest<List<AppRole>> { }
