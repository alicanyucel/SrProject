using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Companies.CreateCompany;
public sealed record CreateCompanyCommand(
    string CompanySmallTitle,
    string CompanyTitle,
    string Representative,
    string PhoneNumber,
    string Email,
    string Address,
    string TaxNo,
    string TaxOffice,
    string WebSite,
    string City,
    string District,
    bool Status,
    int CompanyTypeValue
) : IRequest<Result<string>>;
