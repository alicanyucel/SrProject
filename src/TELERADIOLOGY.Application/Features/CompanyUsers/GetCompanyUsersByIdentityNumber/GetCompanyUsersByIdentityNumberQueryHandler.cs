using MediatR;
using TELERADIOLOGY.Application.Extensions;
using TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUser;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUsersByIdentityNumber;

public class GetCompanyUsersByIdentityNumberQueryHandler(
    IUserRepository userRepository)
    : IRequestHandler<GetCompanyUsersByIdentityNumberQuery, Result<GetCompanyUsersByIdentityNumberResponse>>
{
    public async Task<Result<GetCompanyUsersByIdentityNumberResponse>> Handle(GetCompanyUsersByIdentityNumberQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdentityNumberAsync(request.IdentityNumber, cancellationToken);

        if (user is null)
        {
            return (500, "Kullanıcı bulunamadı");
        }

        var response = new GetCompanyUsersByIdentityNumberResponse(
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber);

        return response;
    }
}

