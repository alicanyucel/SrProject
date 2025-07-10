using MediatR;
using TELERADIOLOGY.Application.Dtos.User;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Users.GetAllUser;

public sealed record GetAllUserQuery() : IRequest<List<UserResultDto>>;
