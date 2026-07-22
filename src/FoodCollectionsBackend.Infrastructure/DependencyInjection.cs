using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FoodCollectionsBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Infrastructure.Identity;
using FoodCollectionsBackend.Infrastructure.Services;
using FoodCollectionsBackend.Application.Interfaces.Auths;
namespace FoodCollectionsBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAppDbContext>(provider =>
            provider.GetRequiredService<AppDbContext>());

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IAuthCommandService, AuthCommandService>();

        return services;
    }
}
