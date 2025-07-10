namespace TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUsersByCompanyId;

public sealed record GetCompanyUsersByCompanyIdQueryResponse(
    Guid? UserId,
    string FirstName,
    string LastName,
    string? Phone,
    string? Email);


