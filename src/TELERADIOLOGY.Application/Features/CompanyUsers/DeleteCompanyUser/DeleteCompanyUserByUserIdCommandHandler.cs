using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.CompanyUsers.DeleteCompanyUser;

public class DeleteCompanyUserByUserIdCommandHandler(
    ICompanyUserRepository companyUserRepository,
    //UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCompanyUserByUserIdCommand, Result<string>>
{

    public async Task<Result<string>> Handle(DeleteCompanyUserByUserIdCommand request, CancellationToken cancellationToken)
    {
        // Kullanıcıyı IdentityNumber ile bul
        var companyUser = await companyUserRepository
            .GetByExpressionWithTrackingAsync(x => x.UserId == request.UserId && x.IsDeleted == false, cancellationToken);

        if (companyUser == null)
        {
            return Result<string>.Failure("Şirket Kullanıcısı bulunamadı.");
        }

        //var user = await userManager.FindByIdAsync(request.UserId.ToString());

        //if (user is null)
        //{
        //    return Result<string>.Failure("Kullanıcı bulunamadı");
        //}

        //user.IsDeleted = true;
        companyUser.IsDeleted = true;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı ve şirket ilişkisi soft delete ile başarıyla silindi.";

    }
}
