using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Roles.RoleSync;

internal sealed class RoleSyncCommandHandler(
 RoleManager<AppRole> roleManager) : IRequestHandler<RoleSyncCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleSyncCommand request, CancellationToken cancellationToken)
    {
        List<AppRole> currentRoles = await roleManager.Roles.ToListAsync(cancellationToken);
        List<AppRole> staticRoles = ConstantsRoles.GetRoles()
            .Select(role => new AppRole
            {
                Id = role.Id,
                Name = role.Name,
                RoleName = role.RoleName,
                NormalizedName = role.NormalizedName,
                Description = role.Description,
                CreatedAt = role.CreatedAt
            }).ToList();

        foreach (var role in currentRoles)
        {
            if (!staticRoles.Any(p => p.Name == role.Name))
            {
                await roleManager.DeleteAsync(role);
            }
        }

        foreach (var role in staticRoles)
        {
            if (!currentRoles.Any(p => p.Name == role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }

        return "Sync is successful";
    }
}
