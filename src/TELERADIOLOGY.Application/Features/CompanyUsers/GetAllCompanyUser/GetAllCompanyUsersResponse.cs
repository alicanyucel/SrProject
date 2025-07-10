namespace TELERADIOLOGY.Application.Features.CompanyUsers.GetAllCompanyUser;

public sealed record GetAllCompanyUsersResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string? Email,
    string? PhoneNumber,
    bool IsActive,
    DateTime? StartDate,
    DateTime? EndDate,
    Guid CompanyId,
    string CompanyTitle);