using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Features.Users.GetUserById;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

internal sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, AppUser>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AppUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetAll().SingleAsync(user => user.Id == request.UserId, cancellationToken);
    }
}
