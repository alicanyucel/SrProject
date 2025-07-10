using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Dtos.User;
using TELERADIOLOGY.Application.Features.Users.FilterUser;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;  

public sealed class FilterUserQueryHandler : IRequestHandler<FilterUserQuery, Result<List<UserResultDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public FilterUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<UserResultDto>>> Handle(FilterUserQuery request, CancellationToken cancellationToken)
    {
        var query = _userRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(request.FilterIdentityNumber))
        {
            var filter = request.FilterIdentityNumber.Trim();
            query = query.Where(u => u.IdentityNumber.Contains(filter));
        }

        query = query.Where(u => !u.IsDeleted);
        var users = await query
            .ProjectTo<UserResultDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<UserResultDto>>.Succeed(users);
    }

   
}
