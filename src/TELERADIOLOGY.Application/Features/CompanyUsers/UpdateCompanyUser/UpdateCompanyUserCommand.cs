using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.CompanyUsers.UpdateCompanyUser;

public record UpdateCompanyUserCommand(
    Guid CompanyId,
    Guid UserId,
    bool IsActive,
    DateTime StartDate,
    DateTime EndDate) : IRequest<Result<string>>;
