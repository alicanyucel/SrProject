using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.CompanyUsers.DeleteCompanyUser;

public record DeleteCompanyUserByUserIdCommand(Guid UserId) : IRequest<Result<string>>;
