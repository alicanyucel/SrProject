using AutoMapper;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Users.UpdateUser;

internal sealed class UpdateUserByIdCommandHandler(
    RoleManager<AppRole> roleManager,
    ICacheService cacheService,
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IMapper mapper) : IRequestHandler<UpdateUserByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {

            // Kullanıcıyı bul
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return Result<string>.Failure("Kullanıcı bulunamadı");
            }

            // Kullanıcı temel bilgilerini güncelle
            mapper.Map(request, user);

            // Şifre değişikliği kontrolü
            if (!string.IsNullOrEmpty(request.Password) && !string.IsNullOrEmpty(request.Repassword))
            {
                if (request.Password != request.Repassword)
                {
                    return Result<string>.Failure("Şifreler eşleşmiyor");
                }

                // Önce mevcut şifre reset token oluştur
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                // Şifreyi değiştir
                var passwordResult = await userManager.ResetPasswordAsync(user, token, request.Password);

                if (!passwordResult.Succeeded)
                {
                    var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    return Result<string>.Failure($"Şifre değiştirilemedi: {errors}");
                }
            }

            // Kullanıcının mevcut rollerini al
            var userRoles = await userManager.GetRolesAsync(user);

            // Mevcut rolleri temizle
            if (userRoles.Any())
            {
                var removeResult = await userManager.RemoveFromRolesAsync(user, userRoles);
                if (!removeResult.Succeeded)
                {
                    var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    return Result<string>.Failure($"Rol kaldırma işlemi başarısız: {errors}");
                }
            }

            // Yeni rolü ID'ye göre bul ve ekle
            var newRole = await roleManager.FindByIdAsync(request.RoleId.ToString());
            if (newRole == null)
            {
                return Result<string>.Failure($"Belirtilen rol bulunamadı (ID: {request.RoleId})");
            }

            var addRoleResult = await userManager.AddToRoleAsync(user, newRole.Name!);
            if (!addRoleResult.Succeeded)
            {
                var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                return Result<string>.Failure($"Rol ekleme işlemi başarısız: {errors}");
            }

            // Kullanıcı bilgilerini güncelle
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                return Result<string>.Failure($"Kullanıcı güncellenemedi: {errors}");
            }

            // Tüm işlemler başarılıysa transaction'ı commit et
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cacheService.Remove("users");
            return Result<string>.Succeed("Kullanıcı başarıyla güncellendi");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Kullanıcı güncelleme sırasında beklenmeyen bir hata oluştu: {ex.Message}");
        }
    }
}
