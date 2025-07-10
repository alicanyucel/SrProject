using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Roles.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<AppRole>>
{
    private readonly RoleManager<AppRole> _roleManager;

    public GetRolesQueryHandler(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task<List<AppRole>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = _roleManager.Roles.ToList();
        return Task.FromResult(roles);
    }
}
