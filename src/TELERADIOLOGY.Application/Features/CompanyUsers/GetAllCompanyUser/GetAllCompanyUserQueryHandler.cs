using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Features.CompanyUsers.GetAllCompanyUser;
using TELERADIOLOGY.Domain.Repositories;

public class GetAllCompanyUsersQueryHandler(
    ICompanyUserRepository companyUserRepository) : IRequestHandler<GetAllCompanyUsersQuery, List<GetAllCompanyUsersResponse>>
{
    public async Task<List<GetAllCompanyUsersResponse>> Handle(GetAllCompanyUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await companyUserRepository
            .Where(cu => !cu.IsDeleted)
            .Include(cu => cu.User)
            .Include(cu => cu.Company)
            .Select(cu => new GetAllCompanyUsersResponse(
                cu.UserId,
                cu.User.FirstName,
                cu.User.LastName,
                cu.User.Email,
                cu.User.PhoneNumber,
                cu.IsActive,
                cu.StartDate,
                cu.EndDate,
                cu.CompanyId,
                cu.Company.CompanyTitle))
            .ToListAsync(cancellationToken);

        return result;
    }
}
