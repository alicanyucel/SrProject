using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TELERADIOLOGY.Application.Features.Users.Createuser;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Users.CreateUser;

public class CreateUserCommandHandler(
    ILoginRepository loginRepository,
    UserManager<AppUser> userManager,
    ICacheService cacheService,
    IUnitOfWork unitOfWork,
    IPasswordHasher<Login> passwordHasher) : IRequestHandler<CreateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
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
            Isactive = request.IsActive,
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
            IsActive = request.IsActive,
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

        appUser.UserRoles.Add(userRole);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        cacheService.Remove("users");
        return Result<string>.Succeed("Kullanıcı eklendi.");
    }
}
