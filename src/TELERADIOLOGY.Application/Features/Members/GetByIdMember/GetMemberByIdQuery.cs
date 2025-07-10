using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Members.GetByIdMember;


public sealed record GetMemberByIdQuery(Guid Id) : IRequest<Member>;
