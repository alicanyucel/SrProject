using MediatR;
using TELERADIOLOGY.Application.Dtos.Company;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Companies.FilterCompany;

public sealed record GetCompaniesByFilterQuery(
    string Filter,
    bool? Status,
    int? Type) : IRequest<Result<List<CompanyResultDto>>>;