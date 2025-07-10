using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Enums;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Companies.UpdateCompany;

internal sealed class UpdateCompanyCommandHandler(
    ICompanyRepository companyRepository,
    ICacheService cacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCompanyCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        // Şirketin var olup olmadığını kontrol et (tracking ile)
        var company = await companyRepository.GetByExpressionWithTrackingAsync(
            c => c.Id == request.Id,
            cancellationToken);

        if (company is null)
        {
            return Result<string>.Failure("Şirket bulunamadı.");
        }

        // Şirket silinmiş mi kontrol et
        if (company.IsDeleted)
        {
            return Result<string>.Failure("Silinmiş şirket güncellenemez.");
        }

        // SmartEnum dönüşümü, geçerli değeri bulur veya hata fırlatabilir:
        CompanyType companyType;
        try
        {
            companyType = CompanyType.FromValue(request.CompanyTypeValue);
        }
        catch (Exception)
        {
            return Result<string>.Failure("Geçersiz CompanyType değeri.");
        }

        // Şirket bilgilerini güncelle
        company.CompanySmallTitle = request.CompanySmallTitle;
        company.CompanyTitle = request.CompanyTitle;
        company.Representative = request.Representative;
        company.PhoneNumber = request.PhoneNumber;
        company.Email = request.Email;
        company.Address = request.Address;
        company.TaxNo = request.TaxNo;
        company.TaxOffice = request.TaxOffice;
        company.WebSite = request.WebSite;
        company.City = request.City;
        company.District = request.District;
        company.Status = request.Status;
        company.CompanyType = companyType;
        // company.UpdatedAt = DateTime.UtcNow; // Eğer UpdatedAt alanınız varsa

        // Repository'nizde Update metodu var, ama tracking ile aldığımız için EF otomatik takip ediyor
        // Dolayısıyla Update() çağrısına gerek yok, sadece SaveChanges yeterli

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cacheService.Remove("companies");
            return "Şirket bilgileri güncellendi.";
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.InnerException?.Message ?? ex.Message);
        }
    }
}