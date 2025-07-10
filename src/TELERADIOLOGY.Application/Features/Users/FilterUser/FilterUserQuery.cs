using MediatR;
using TELERADIOLOGY.Application.Dtos.User;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Users.FilterUser;

public  record FilterUserQuery(
string FilterIdentityNumber
) : IRequest<Result<List<UserResultDto>>>;
