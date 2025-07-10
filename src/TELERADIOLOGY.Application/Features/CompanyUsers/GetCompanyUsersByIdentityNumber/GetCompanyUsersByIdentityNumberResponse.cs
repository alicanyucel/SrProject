namespace TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUsersByIdentityNumber;
public sealed record GetCompanyUsersByIdentityNumberResponse(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Email
    );
