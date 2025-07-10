using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Companies.DeleteCompany;

public sealed class DeleteCompanyByIdCommandHandler(
    ICompanyRepository companyRepository,
    ICacheService cacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCompanyByIdCommand, Result<string>>
{

    public async Task<Result<string>> Handle(DeleteCompanyByIdCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByExpressionWithTrackingAsync(x => x.Id == request.CompanyId, cancellationToken);

        if (company is null)
        {
            return Result<string>.Failure("Şirket bulunamadı.");
        }

        company.IsDeleted = true;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        cacheService.Remove("companies");

        return Result<string>.Succeed("Şirket başarıyla silindi.");
    }
}
