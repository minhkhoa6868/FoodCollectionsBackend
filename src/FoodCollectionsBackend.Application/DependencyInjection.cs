using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FoodCollectionsBackend.Application.Common.Behaviors;

namespace FoodCollectionsBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        // Integrate logging
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));

        services.AddAutoMapper(cfg => {}, typeof(DependencyInjection).Assembly);

        return services;
    }
}
