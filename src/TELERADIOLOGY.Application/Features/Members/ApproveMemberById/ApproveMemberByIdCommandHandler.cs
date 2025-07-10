using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.ApproveMemberById;

internal sealed class ApproveMemberByIdCommandHandler(
    IMemberRepository memberRepository,
    ILoginRepository loginRepository,
    UserManager<AppUser> userManager,
    ICacheService cacheService,
    IUnitOfWork unitOfWork,
    IPasswordHasher<Login> passwordHasher) : IRequestHandler<ApproveMemberByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ApproveMemberByIdCommand request, CancellationToken cancellationToken)
    {

        Member? member = await memberRepository.GetByExpressionWithTrackingAsync(x => x.Id == request.Id, cancellationToken);

        if (member is null)
        {
            return Result<string>.Failure(500, "Başvuru bulunamadı");
        }

        if (request.Password != request.Repassword)
        {
            return Result<string>.Failure(400, "Şifreler eşleşmiyor.");
        }

        var login = new Login
        {
            LoginId = Guid.NewGuid(),
            RoleId = request.RoleId,
            UserName = request.Email,
            Password = request.Password,
            Isactive = true,
            CreatedDate = DateTime.Now,
            LastLogin = DateTime.Now,
            UserCode = Guid.NewGuid().ToString("N").Substring(0, 8)
            //to do kalkçak
        };

        login.Password = passwordHasher.HashPassword(login, request.Password);
        await loginRepository.AddAsync(login, cancellationToken);

        var appUser = new AppUser
        {
            LoginId = login.LoginId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            UserName = login.UserName,
            Email = request.Email,
            IdentityNumber = request.IdentityNumber,
            EmailConfirmed = true,
            PhoneNumber = request.PhoneNumber,
            PhoneNumberConfirmed = true,
            UserRoles = new List<AppUserRole>()
        };
        await userManager.CreateAsync(appUser, request.Password);

        var userRole = new AppUserRole
        {
            UserId = appUser.Id,
            RoleId = request.RoleId
        };

        member.ApplicationStatus = Domain.Enums.ApplicationStatus.Approved;

        appUser.UserRoles.Add(userRole);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        cacheService.Remove("users");
        cacheService.Remove("members");
        return Result<string>.Succeed("Başvuru onaylandı, kullanıcı başarıyla kayıt edildi");
    }
}
