using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.GetAllMember;

public sealed record GetAllMemberQuery() : IRequest<Result<List<Member>>>;