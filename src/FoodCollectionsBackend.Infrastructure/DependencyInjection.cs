using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FoodCollectionsBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Infrastructure.Identity;
using FoodCollectionsBackend.Infrastructure.Services;
using FoodCollectionsBackend.Application.Interfaces.Auths;
using FoodCollectionsBackend.Infrastructure.Services.Auths;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Interfaces.Users;
using FoodCollectionsBackend.Infrastructure.Services.Users;
namespace FoodCollectionsBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        // Register AppDbContext as IAppDbContext
        services.AddScoped<IAppDbContext>(provider =>
            provider.GetRequiredService<AppDbContext>());

        // Redis Cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:ConnectionString"];
        });

        // Current User Service
        services.AddScoped<ICurrentUser, CurrentUser>();

        // Jwt Service
        services.AddScoped<IJwtService, JwtService>();

        // Password Hasher Service
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Email Service
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();
        services.AddMemoryCache();
        services.AddSingleton<OtpService>();

        // Cache Service
        services.AddScoped<ICacheService, CacheService>();

        // Auth Service
        services.AddScoped<IAuthCommandService, AuthCommandService>();

        // User Service
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<IUserQueryService, UserQueryService>();

        return services;
    }
}
