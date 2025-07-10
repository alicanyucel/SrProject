using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Features.Users.GetAllRolesForUsers;
using TELERADIOLOGY.Domain.Entities;

public class GetAllUserRolesQueryHandler : IRequestHandler<GetAllUserRolesQuery, List<AppUserRole>>
{
    private readonly IUserRoleRepository _userroleRepository;

    public GetAllUserRolesQueryHandler(IUserRoleRepository userroleRepository)
    {
        _userroleRepository = userroleRepository;
    }

    public async Task<List<AppUserRole>> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
    {
  
        var userRoles = await _userroleRepository.GetAll()
            .OrderBy(x => x.UserId)   
            .Select(x => new AppUserRole
            {
                UserId = x.UserId,               
                RoleId = x.RoleId,          
                     
            })
            .ToListAsync(cancellationToken); 
        return userRoles;
    }
}
