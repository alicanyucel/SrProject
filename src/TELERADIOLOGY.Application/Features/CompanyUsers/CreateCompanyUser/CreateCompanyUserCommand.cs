using MediatR;
using TS.Result;

public record CreateCompanyUserCommand(
    Guid CompanyId,
    string IdentityNumber,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool IsActive,
    DateTime StartDate,
    DateTime? EndDate) : IRequest<Result<string>>;


