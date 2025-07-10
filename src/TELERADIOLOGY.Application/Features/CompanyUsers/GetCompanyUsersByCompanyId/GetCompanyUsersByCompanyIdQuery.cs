using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUsersByCompanyId;
public sealed record GetCompanyUsersByCompanyIdQuery(
    Guid CompanyId) : IRequest<Result<List<GetCompanyUsersByCompanyIdQueryResponse>>>;
