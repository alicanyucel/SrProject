using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Application.Constants;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

public class CreateCompanyUserCommandHandler(
    ICompanyRepository companyRepository,
    IUserRepository userRepository,
    ICompanyUserRepository companyUserRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCompanyUserCommand, Result<string>>
{

    public async Task<Result<string>> Handle(CreateCompanyUserCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository
            .WhereWithTracking(x => x.Id == request.CompanyId)
            .Include(c => c.CompanyUsers)
            .SingleOrDefaultAsync(cancellationToken);

        if (company is null)
            return Result<string>.Failure("Şirket bulunamadı.");

        var user = await userRepository
            .GetByExpressionAsync(x => x.IdentityNumber == request.IdentityNumber, cancellationToken);

        if (user is null)
        {
            user = new AppUser
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdentityNumber = request.IdentityNumber,
                Email = request.Email,
                Phone = request.PhoneNumber,
                UserRoles = new List<AppUserRole>(),
                IsActive = request.IsActive
            };

            await userRepository.AddAsync(user, cancellationToken);
        }

        // Firma rolünü ata
        if (!user.UserRoles.Any(r => r.RoleId == CompanyRoleConstants.Company))
        {
            user.UserRoles.Add(new AppUserRole
            {
                RoleId = CompanyRoleConstants.Company,
                UserId = user.Id
            });
        }

        var alreadyExists = await companyUserRepository
            .AnyAsync(x => x.CompanyId == request.CompanyId && x.UserId == user.Id, cancellationToken);

        if (alreadyExists)
        {
            return Result<string>.Failure("Bu kullanıcı zaten bu şirkete eklenmiş");
        }

        // 4. CompanyUser ilişkisi oluştur
        var companyUser = new CompanyUser
        {
            CompanyId = request.CompanyId,
            UserId = user.Id,
            IsActive = request.IsActive,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
        };

        // company.CompanyUsers.Add(companyUser);
        await companyUserRepository.AddAsync(companyUser, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Kullanıcı şirkete başarıyla eklendi.";
    }
}
