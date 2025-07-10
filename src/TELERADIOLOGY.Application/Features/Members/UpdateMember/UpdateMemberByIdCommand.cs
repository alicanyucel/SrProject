using MediatR;
using TELERADIOLOGY.Domain.Enums;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.UpdateMember;

public sealed record UpdateMemberByIdCommand(Guid Id,string Name, string SurName, string Phone, string Email, ApplicationStatus ApplicationStatus, AreaOfInterest AreaOfInterest, DateTime ApplicationDate) : IRequest<Result<string>>;
