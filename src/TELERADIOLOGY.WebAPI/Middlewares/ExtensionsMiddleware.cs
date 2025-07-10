using Microsoft.AspNetCore.Identity;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Infrastructure.Context;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var dbContext = scoped.ServiceProvider.GetRequiredService<PostgreSqlDbContext>();

        if (!userManager.Users.Any(p => p.UserName == "admin"))
        {
            var existingLogin = dbContext.Logins.FirstOrDefault(l => l.UserName == "admin");
            Guid loginId;

            if (existingLogin == null)
            {
                loginId = Guid.NewGuid();

                var login = new Login
                {
                    LoginId = loginId,
                    UserName = "admin",
                    RoleId = Guid.Parse("bc4bfbbd-f021-45b4-a175-7064d2ade7ae"),
                    Isactive = true,
                    CreatedDate = DateTime.UtcNow
                };

                dbContext.Logins.Add(login);
                dbContext.SaveChanges();
            }
            else
            {
                loginId = existingLogin.LoginId;
            }

            var user = new AppUser
            { 
                UserName = "admin",
                FirstName = "NetCare",
                LastName = "Sağlık Bilişim",
                Email = "yucelalicandan@hotmail.com",
                Phone = "05416923675",
                IdentityNumber = "13186116052",
                LoginId = loginId,
                PhoneNumber = "05416923675",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
            };

            var result = userManager.CreateAsync(user, "Admin123*").Result;

            if (!result.Succeeded)
            {
                throw new Exception("Admin kullanıcısı oluşturulamadı.: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
