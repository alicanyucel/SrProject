using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TELERADIOLOGY.Application.Behaviors;

namespace TELERADIOLOGY.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
            conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
            conf.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddHttpContextAccessor();
        return services;
    }
}
