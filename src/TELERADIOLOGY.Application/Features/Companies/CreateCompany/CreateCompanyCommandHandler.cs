using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Enums;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Companies.CreateCompany;
internal sealed class CreateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    ICacheService cacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCompanyCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        
        CompanyType companyType;

        try
        {
            companyType = CompanyType.FromValue(request.CompanyTypeValue);
        }
        catch (Exception)
        {
            return Result<string>.Failure("Geçersiz CompanyType değeri.");
        }

        var company = new Company
        {
            CompanySmallTitle = request.CompanySmallTitle,
            CompanyTitle = request.CompanyTitle,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Address = request.Address,
            TaxNo = request.TaxNo,
            TaxOffice = request.TaxOffice,
            Representative = request.Representative,
            WebSite = request.WebSite,
            City = request.City,
            District = request.District,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            CompanyType = companyType
        };

        await companyRepository.AddAsync(company, cancellationToken);

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cacheService.Remove("companies");
            return "Şirket kaydı yapıldı.";
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.InnerException?.Message ?? ex.Message);
        }
    }
}