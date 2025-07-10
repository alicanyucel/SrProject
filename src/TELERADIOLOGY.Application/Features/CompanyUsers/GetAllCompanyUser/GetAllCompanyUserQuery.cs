using MediatR;

namespace TELERADIOLOGY.Application.Features.CompanyUsers.GetAllCompanyUser;

public record GetAllCompanyUsersQuery() : IRequest<List<GetAllCompanyUsersResponse>>;
