using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Dtos.User;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Application.Features.Users.GetAllUser;

internal sealed class GetAllUserQueryHandler(
    IUserRepository userRepository,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<GetAllUserQuery, List<UserResultDto>>
{
    //
    public async Task<List<UserResultDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        List<AppUser>? users;

        users = cacheService.Get<List<AppUser>>("users");

        if (users is null)
        {
            users =
                await userRepository.GetAll()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted && u.LoginId != null)
            .OrderBy(u => u.FirstName)
            .ToListAsync(cancellationToken);

            cacheService.Set<List<AppUser>>("users", users);
        }

        var userDto = mapper.Map<List<UserResultDto>>(users);

        return userDto;
    }
}
