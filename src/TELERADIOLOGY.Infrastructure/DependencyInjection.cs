using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Infrastructure.Context;
using TELERADIOLOGY.Infrastructure.Options;
using TELERADIOLOGY.Infrastructure.Services;

namespace TELERADIOLOGY.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<PostgreSqlDbContext>());
        services
            .AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<PostgreSqlDbContext>()
            .AddDefaultTokenProviders();

        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtTokenOptionsSetup>();

        services.AddAuthentication()
            .AddJwtBearer();

        services.AddAuthorizationBuilder();


        services.Scan(action =>
        {
            action
                .FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        return services;
    }
}