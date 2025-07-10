using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.CompanyUsers.UpdateCompanyUser;

public class UpdateCompanyUserCommandHandler(
    ICompanyUserRepository companyUserRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCompanyUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCompanyUserCommand request, CancellationToken cancellationToken)
    {
        var companyUser = await companyUserRepository
            .WhereWithTracking(x => x.CompanyId == request.CompanyId && x.UserId == request.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (companyUser is null || companyUser.IsDeleted)
        {
            return Result<string>.Failure("Bu kullanıcı-şirket ilişkisi bulunamadı");
        }

        companyUser.CompanyId = request.CompanyId;
        companyUser.UserId = request.UserId;
        companyUser.IsActive = request.IsActive;
        companyUser.StartDate = request.StartDate;
        companyUser.EndDate = request.EndDate;
        companyUser.UpdatedAt = DateTime.UtcNow;

        companyUserRepository.Update(companyUser);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı-şirket ilişkisi başarıyla güncellendi.";
    }
}